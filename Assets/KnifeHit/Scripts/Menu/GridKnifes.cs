
using UnityEngine;

namespace KnifeHit.Scripts.Menu
{
    public class GridKnifes : MonoBehaviour
    {
        [SerializeField] private GridItem gridItemPrefab;
        [SerializeField] private Transform gridItemParent;
        [SerializeField] private CollectionBuyItems collectionBuyItems;
        [SerializeField] private GridSelector gridSelector;


        private void Start()
        {
            RemoveOldItems();

            GridItem firstSelected = null;
            foreach (var item in collectionBuyItems.ItemBuyInfos)
            {
                var newItem = Instantiate(gridItemPrefab, gridItemParent);
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
            var items = gridItemParent.GetComponentsInChildren<GridItem>();
            foreach (var item in items)
            {
                Destroy(item.gameObject);
            }
        }
    }
}