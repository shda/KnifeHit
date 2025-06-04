using System;
using Lua;
using UnityEngine;

//[MoonSharpUserData]
[LuaObject]
public partial class TargetLuaConnector
{
    [LuaMember]
    public void Call()
    {
        
    }
}

public class LuaLevel : MonoBehaviour
{
    [SerializeField] private TextAsset levelData;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
       // CreateLua();
        
        /*
        var state = LuaState.Create();
        state.Environment["targetLuaConnector"] = new TargetLuaConnector();
        
        var results = await state.DoStringAsync(levelData.text);

        var table = state.Environment["rotations"].Read<LuaTable>();

        var memory =  table.GetArrayMemory();
        
        for (int i = 1; i < table.ArrayLength + 1; i++)
        {
            var row = table[i];
        }

        /*
        var  v = state.Environment["targetLuaConnector"];

        if (state.Environment["Test"].TryRead(out LuaFunction function))
        {
            var funcResults = await function.InvokeAsync(state , Array.Empty<LuaValue>());
        }
        */
    }


    public async void CreateLua()
    {
        var state = LuaState.Create();
        state.Environment["setRotations"] = new LuaFunction((context, buffer, ct) =>
        {
            // Get the arguments using context.GetArgument<T>()
            var listRotations = context.GetArgument<LuaTable>(0);

            var mem = listRotations.GetArrayMemory();
            foreach (var val in mem.ToArray())
            {
                
            }

            return new(0);
        });
        
        var results = await state.DoStringAsync(levelData.text);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
