using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using KnifeHit.Scripts.Levels;
using KnifeHit.Scripts.Lists;
using Lua;
using Lua.Unity;
using UnityEngine;

namespace KnifeHit.Scripts.LuaLogic
{
    public class LuaScriptLoader : MonoBehaviour
    {
        [SerializeField] private LuaAsset luaAsset;
        [SerializeField] private LevelLuaProxy  levelLuaProxy;

        private LuaState _luaState;

        private CancellationTokenSource _cancellation;

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
                LoadLevelFromLuaLogic(luaAsset.Text , _cancellation.Token);
                PlayerPrefs.SetString(LevelEditorController.NameSave, luaAsset.Text);
            }
        }

        private async void LoadLevelFromLuaLogic(string luaCode, CancellationToken token)
        {
            await UniTask.Yield();

            try
            {
                _luaState = LuaState.Create();

                _luaState.Environment["level"] = levelLuaProxy;
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