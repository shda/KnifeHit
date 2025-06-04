using System;
using System.Threading;
using System.Threading.Tasks;
using BlockBlast.Scripts.Common.Extensions;
using Cysharp.Threading.Tasks;
using KnifeHit.Scripts.Levels;
using KnifeHit.Scripts.Lists;
using Lua;
using Lua.Unity;
using TMPro;
using UnityEngine;

namespace KnifeHit.Scripts.LuaLogic
{
    public class LuaLevelLogic : MonoBehaviour
    {
        [SerializeField] private LevelPlayer levelPlayer;
        [SerializeField] private LuaAsset luaAsset;
        
        [SerializeField] private ListBonuses  listBonuses;
        [SerializeField] private ListKnifes listKnifes;
        [SerializeField] private Target target;

        private LuaState _luaState;

        private CancellationTokenSource _cancellation;
        
        private void Awake()
        {
            LoadLevelFromLuaLogic(luaAsset.Text);
        }

        public async void LoadLevelFromLuaLogic(string luaCode)
        {
            _cancellation?.Cancel();
            _cancellation = new CancellationTokenSource();
            
            _luaState = LuaState.Create();
            
            _luaState.Environment["rotateAsync"] = new LuaFunction(RotateAsync);
            _luaState.Environment["setUserKnifeSkin"] = new LuaFunction(SetUserKnifeSkin);
            _luaState.Environment["setTargetSkin"] = new LuaFunction(SetTargetSkin);
            _luaState.Environment["setBonus"] = new LuaFunction(SetBonus);
            _luaState.Environment["setObstacle"] = new LuaFunction(SetObstacle);
            
            var results = await _luaState.DoStringAsync(luaCode , cancellationToken: _cancellation.Token);
            var func = results[0].Read<LuaFunction>();

            while (!_cancellation.Token.IsCancellationRequested)
            {
                await func.InvokeAsync(_luaState, Array.Empty<LuaValue>() , cancellationToken:_cancellation.Token);
                await UniTask.Yield();
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
            Debug.Log("SetObstacle");
            
            var methodParameters = GetMethodParameters(context);
            var knife = Instantiate(listKnifes.GetWithOverflow(methodParameters.IndexObject));
            knife.IsMoving = false;
            knife.SetStaticRigidbody2D();
            
            target.AddObject(knife.gameObject ,  methodParameters.Angle , 180);
            
            return new (0);
        }

        private ValueTask<int> SetBonus(
            LuaFunctionExecutionContext context, 
            Memory<LuaValue> values, CancellationToken cancellation)
        {
            Debug.Log("SetBonus");

           // var bonusNumber = context.GetArgument<string>(0).ToInt();
           // var angle = context.GetArgument<int>(1);

            var methodParameters = GetMethodParameters(context);

            var bonus = Instantiate(listBonuses.GetWithOverflow(methodParameters.IndexObject));
            target.AddObject(bonus.gameObject , methodParameters.Angle);
            
           // Debug.Log("SetBonus "  + bonusNumber);
            
            return new (0);
        }

        private ValueTask<int> SetTargetSkin(
            LuaFunctionExecutionContext context, 
            Memory<LuaValue> values, CancellationToken cancellation)
        {
            Debug.Log("SetTargetSkin");
            
            return new (0);
        }

        private ValueTask<int> SetUserKnifeSkin(
            LuaFunctionExecutionContext context, 
            Memory<LuaValue> values, CancellationToken cancellation)
        {
            Debug.Log("SetUserKnifeSkin");
            
            return new (0);
        }

        private async ValueTask<int> RotateAsync(
            LuaFunctionExecutionContext context,
            Memory<LuaValue> values, CancellationToken cancellation)
        {
            var listRotations = context.GetArgument<string>(0);
            var result = LevelParser.ParseLine(listRotations);
            
            Debug.Log(listRotations);

            if (cancellation.IsCancellationRequested)
                return 0;
            
            await levelPlayer.PlayStep(result, cancellation);
            return 0;
        }
        
        
    }
}
