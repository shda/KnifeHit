using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using KnifeHit.Scripts.Levels;
using Lua;
using Lua.Unity;
using UnityEngine;

namespace KnifeHit.Scripts.LuaLogic
{
    public class LuaScriptLoader 
    {
        private LevelLuaProxy  _LevelLuaProxy;
        private LuaState _luaState;

        private CancellationTokenSource _cancellation;
        private readonly GameSettings _gameSettings;

        public LuaScriptLoader(GameSettings gameSettings , LevelLuaProxy  levelLuaProxy)
        {
            _gameSettings = gameSettings;
            _LevelLuaProxy = levelLuaProxy;
        }
        
        public void StartLevel()
        {
            _cancellation?.Cancel();
            _cancellation = new CancellationTokenSource();
           
            
            var text = PlayerPrefs.GetString(LevelEditorController.NameSave);
            if (!string.IsNullOrEmpty(text))
            {
                LoadLevelFromLuaLogic(text , _cancellation.Token);
            }
            else
            {
                LoadLevelFromLuaLogic(_gameSettings.LuaAsset.Text , _cancellation.Token);
                PlayerPrefs.SetString(LevelEditorController.NameSave, _gameSettings.LuaAsset.Text);
            }
        }

        private async void LoadLevelFromLuaLogic(string luaCode, CancellationToken token)
        {
            await UniTask.Yield();

            try
            {
                _luaState = LuaState.Create();

                _luaState.Environment["level"] = _LevelLuaProxy;
                await _luaState.DoStringAsync(luaCode, cancellationToken: token);
            }
            catch (LuaCanceledException luaCanceled)
            {
                Debug.Log(luaCanceled.Message);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        
        private void OnDestroy()
        {
            _cancellation?.Cancel();
        }

        public void StopLevel()
        {
            _cancellation?.Cancel();
        }
    }
}