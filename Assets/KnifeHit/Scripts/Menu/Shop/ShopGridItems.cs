using System;
using UnityEngine;
using Zenject;

namespace KnifeHit.Scripts.Menu.Shop
{
    public class ShopGridItems : MonoBehaviour
    {
        [SerializeField] private ShopItem shopItemPrefab;
        [SerializeField] private Transform gridItemParent;
        [SerializeField] private CollectionMarketItems collectionMarketItems;
        [SerializeField] private GridSelector gridSelector;

        public Action<ShopItem> OnPressShopItem;
        
        private GameStats _gameStats;

        [Inject]
        public void Construct(GameStats gameStats)
        {
            _gameStats = gameStats;
        }
        
        private void Start()
        {
            RemoveOldItems();

            ShopItem firstSelected = null;
            foreach (var item in collectionMarketItems.ItemBuyInfos)
            {
                var newItem = Instantiate(shopItemPrefab, gridItemParent);
                newItem.transform.localPosition = Vector3.zero;
                newItem.transform.localScale = Vector3.one;
                newItem.SetInfo(item , _gameStats);
                newItem.OnPress = OnPress;
                
                if(!firstSelected)
                    firstSelected = newItem;
            }

            OnPress(firstSelected);
        }

        private void OnPress(ShopItem gridItem)
        {
            OnPressShopItem?.Invoke(gridItem);
            gridSelector.SetTargetRect(gridItem.GetComponent<RectTransform>());
        }

        private void RemoveOldItems()
        {
            var items = gridItemParent.GetComponentsInChildren<ShopItem>();
            foreach (var item in items)
            {
                Destroy(item.gameObject);
            }
        }
    }
}