using UnityEngine;

namespace KnifeHit.Scripts.Menu.Shop
{
    [CreateAssetMenu(menuName = "Create CollectionMarketItems", fileName = "CollectionMarketItems", order = 0)]
    public class CollectionMarketItems : ScriptableObject
    {
        [SerializeField] private ItemInfo[] itemBuyInfos;
        
        public ItemInfo[] ItemBuyInfos => itemBuyInfos;
    }
}