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
        [SerializeField] private float delayNextKnife;
        
        public LuaAsset LuaAsset => luaAsset;
        public ListBonuses ListBonuses => listBonuses;
        public float DelayNextKnife => delayNextKnife;
        
    }
}