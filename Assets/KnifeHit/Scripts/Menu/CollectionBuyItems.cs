using UnityEngine;

namespace KnifeHit.Scripts.Menu
{
    [CreateAssetMenu(menuName = "Create CollectionBuyItems", fileName = "CollectionBuyItems", order = 0)]
    public class CollectionBuyItems : ScriptableObject
    {
        [SerializeField] private ItemInfo[] itemBuyInfos;
        
        public ItemInfo[] ItemBuyInfos => itemBuyInfos;
    }
}