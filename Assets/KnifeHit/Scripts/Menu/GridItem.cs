using System;
using UnityEngine;
using UnityEngine.UI;

namespace KnifeHit.Scripts.Menu
{
    public class GridItem : MonoBehaviour
    {
        [SerializeField] private Image image;
        
        public bool IsOpen { get; set; }
        public ItemInfo ItemInfo { get; set; }
        
        public Action<GridItem> OnPress;

        public void OnPressItem()
        {
            OnPress?.Invoke(this);
        }

        public void SetInfo(ItemInfo itemInfo)
        {
            ItemInfo = itemInfo;
            image.sprite = itemInfo.ShadowKnifeSprite;
        }
    }
}