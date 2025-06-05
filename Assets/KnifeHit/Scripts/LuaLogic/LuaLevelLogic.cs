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
    public class LuaLevelLogic : MonoBehaviour
    {
        [SerializeField] private LevelPlayer levelPlayer;
        [SerializeField] private LuaAsset luaAsset;

        [SerializeField] private ListBonuses listBonuses;
        [SerializeField] private ListKnifes listKnifes;
        [SerializeField] private Target target;
        [SerializeField] private Game game;

        private LuaState _luaState;

        private CancellationTokenSource _cancellation;

        public void StartLevel()
        {
            _cancellation?.Cancel();
            _cancellation = new CancellationTokenSource();
           
            
            var text = PlayerPrefs.GetString(LevelMaker.NameSave);
            if (!string.IsNullOrEmpty(text))
            {
                LoadLevelFromLuaLogic(text , _cancellation.Token);
            }
            else
            {
                LoadLevelFromLuaLogic(luaAsset.Text , _cancellation.Token);
                PlayerPrefs.SetString(LevelMaker.NameSave, luaAsset.Text);
            }
        }

        public async void LoadLevelFromLuaLogic(string luaCode, CancellationToken token)
        {
            await UniTask.Yield();

            try
            {
                _luaState = LuaState.Create();
                _luaState.Environment["rotateAsync"] = new LuaFunction(RotateAsync);
                _luaState.Environment["setUserKnifeSkin"] = new LuaFunction(SetUserKnifeSkin);
                _luaState.Environment["setTargetSkin"] = new LuaFunction(SetTargetSkin);
                _luaState.Environment["setBonus"] = new LuaFunction(SetBonus);
                _luaState.Environment["setObstacle"] = new LuaFunction(SetObstacle);

                var results = await _luaState.DoStringAsync(luaCode, cancellationToken: token);
                var func = results[0].Read<LuaFunction>();

                while (!token.IsCancellationRequested)
                {
                    await func.InvokeAsync(_luaState, Array.Empty<LuaValue>(), cancellationToken: token);
                    await UniTask.Yield();
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private LuaObjParam GetMethodParameters(LuaFunctionExecutionContext context)
        {
            var param = new LuaObjParam();

            if (context.ArgumentCount > 0)
            {
                param.IndexObject = context.GetArgument<int>(0);
                //param.IndexObject--;
            }

            if (context.ArgumentCount > 1)
            {
                param.Angle = context.GetArgument<int>(1);
            }

            if (context.ArgumentCount > 2)
            {
                param.AddOffset = context.GetArgument<float>(2);
            }

            return param;
        }

        private ValueTask<int> SetObstacle(
            LuaFunctionExecutionContext context,
            Memory<LuaValue> values, CancellationToken cancellation)
        {
            if (cancellation.IsCancellationRequested)
                return new ValueTask<int>(0);

            Debug.Log("SetObstacle");

            var methodParameters = GetMethodParameters(context);
            var knife = Instantiate(listKnifes.GetWithOverflow(methodParameters.IndexObject));
            knife.IsMoving = false;
            knife.SetStaticRigidbody2D();

            target.AddObject(knife.gameObject, methodParameters.Angle, 180);

            return new(0);
        }

        private ValueTask<int> SetBonus(
            LuaFunctionExecutionContext context,
            Memory<LuaValue> values, CancellationToken cancellation)
        {
            if (cancellation.IsCancellationRequested)
                return new ValueTask<int>(0);

            Debug.Log("SetBonus");
            var methodParameters = GetMethodParameters(context);
            var bonus = Instantiate(listBonuses.GetWithOverflow(methodParameters.IndexObject));
            target.AddObject(bonus.gameObject, methodParameters.Angle);

            return new(0);
        }

        private ValueTask<int> SetTargetSkin(
            LuaFunctionExecutionContext context,
            Memory<LuaValue> values, CancellationToken cancellation)
        {
            if (cancellation.IsCancellationRequested)
                return new ValueTask<int>(0);

            var skinIndex = GetMethodParameters(context);
            target.SetSkin(skinIndex.IndexObject);
            Debug.Log("SetTargetSkin " + skinIndex.IndexObject);

            return new(0);
        }

        private ValueTask<int> SetUserKnifeSkin(
            LuaFunctionExecutionContext context,
            Memory<LuaValue> values, CancellationToken cancellation)
        {
            if (cancellation.IsCancellationRequested)
                return new ValueTask<int>(0);

            Debug.Log("SetUserKnifeSkin");

            var skinIndex = GetMethodParameters(context);
            game.SetKnifeSkin(skinIndex.IndexObject);

            return new(0);
        }

        private async ValueTask<int> RotateAsync(
            LuaFunctionExecutionContext context,
            Memory<LuaValue> values, CancellationToken cancellation)
        {
            await UniTask.Yield();
            Debug.Log("Rotate");

            if (cancellation.IsCancellationRequested)
                return 0;

            var listRotations = context.GetArgument<string>(0);
            var result = LevelParser.ParseLine(listRotations);

            Debug.Log(listRotations);

            if (cancellation.IsCancellationRequested)
                return 0;

            await levelPlayer.PlayStep(result, cancellation);
            return 0;
        }

        private void OnDestroy()
        {
            _cancellation?.Cancel();
        }
    }
}