using UnityEngine;

namespace KnifeHit.Scripts.Menu
{
    public class ItemBuyInfo
    {
        [SerializeField] private Sprite knifeSprite;
        [SerializeField] private Sprite shadowKnifeSprite;
        
        public Sprite KnifeSprite => knifeSprite;
        public Sprite ShadowKnifeSprite => shadowKnifeSprite;
    }

    public class CollectionBuyItems : ScriptableObject
    {
        [SerializeField] private ItemBuyInfo[] itemBuyInfos;
        
        public ItemBuyInfo[] ItemBuyInfos => itemBuyInfos;
    }
    
    public class GridSelector : MonoBehaviour
    {
        
    }
    
    public class GridSelectedKnifes : MonoBehaviour
    {
        
    }
}