
using UnityEngine;

namespace KnifeHit.Scripts.Menu
{
    public class ShopGridItems : MonoBehaviour
    {
        [SerializeField] private ShopItem shopItemPrefab;
        [SerializeField] private Transform gridItemParent;
        [SerializeField] private CollectionMarketItems collectionMarketItems;
        [SerializeField] private GridSelector gridSelector;


        private void Start()
        {
            RemoveOldItems();

            ShopItem firstSelected = null;
            foreach (var item in collectionMarketItems.ItemBuyInfos)
            {
                var newItem = Instantiate(shopItemPrefab, gridItemParent);
                newItem.transform.localPosition = Vector3.zero;
                newItem.transform.localScale = Vector3.one;
                newItem.SetInfo(item);
                newItem.OnPress = gridItem =>
                {
                    gridSelector.SetTargetRect(gridItem.GetComponent<RectTransform>());
                };
                
                if(!firstSelected)
                    firstSelected = newItem;
            }

            gridSelector.SetTargetRect(firstSelected.GetComponent<RectTransform>());
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