using System;
using System.Threading;
using System.Threading.Tasks;
using BlockBlast.Scripts.Game;
using Cysharp.Threading.Tasks;
using KnifeHit.Scripts.Levels;
using Lua;
using Lua.Unity;
using UnityEngine;

namespace KnifeHit.Scripts.LuaLogic
{
    public class LuaLevelLogic : MonoBehaviour
    {
        [SerializeField] private LevelPlayer levelPlayer;
        [SerializeField] private LuaAsset luaAsset;

        private LuaState _luaState;

        private void Awake()
        {
            LoadLevelFromLuaLogic(luaAsset.Text);
        }

        public async void LoadLevelFromLuaLogic(string luaCode)
        {
            _luaState = LuaState.Create();
            _luaState.Environment["rotateAsync"] = new LuaFunction(RotateAsync);
            _luaState.Environment["setUserKnifeSkin"] = new LuaFunction(SetUserKnifeSkin);
            _luaState.Environment["setTargetSkin"] = new LuaFunction(SetTargetSkin);
            _luaState.Environment["setBonus"] = new LuaFunction(SetBonus);
            _luaState.Environment["setObstacle"] = new LuaFunction(SetObstacle);
            
            var results = await _luaState.DoStringAsync(luaCode);
            var func = results[0].Read<LuaFunction>();
            
            await func.InvokeAsync(_luaState, Array.Empty<LuaValue>());
        }

        private ValueTask<int> SetObstacle(
            LuaFunctionExecutionContext context, 
            Memory<LuaValue> values, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        private ValueTask<int> SetBonus(
            LuaFunctionExecutionContext context, 
            Memory<LuaValue> values, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        private ValueTask<int> SetTargetSkin(
            LuaFunctionExecutionContext context, 
            Memory<LuaValue> values, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        private ValueTask<int> SetUserKnifeSkin(
            LuaFunctionExecutionContext context, 
            Memory<LuaValue> values, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        private async ValueTask<int> RotateAsync(
            LuaFunctionExecutionContext context,
            Memory<LuaValue> values, CancellationToken cancellation)
        {
            var listRotations = context.GetArgument<string>(0);
            var result = LevelParser.ParseLine(listRotations);
            Debug.Log(listRotations);
            await levelPlayer.PlayStep(result, cancellation);
            return 0;
        }
        
        
    }
}
