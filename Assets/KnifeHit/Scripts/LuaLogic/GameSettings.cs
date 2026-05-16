using KnifeHit.Scripts.Lists;
using Lua.Unity;
using UnityEngine;

namespace KnifeHit.Scripts.LuaLogic
{
    [CreateAssetMenu(menuName = "Create GameSettings", fileName = "GameSettings", order = 0)]
    public class GameSettings : ScriptableObject
    {
        [SerializeField] private LuaAsset luaAsset;
        [SerializeField] private ListBonuses listBonuses;
        
        public LuaAsset LuaAsset => luaAsset;
        public ListBonuses ListBonuses => listBonuses;
        
    }
}