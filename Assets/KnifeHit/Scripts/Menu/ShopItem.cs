using System;
using UnityEngine;
using UnityEngine.UI;

namespace KnifeHit.Scripts.Menu
{
    public class ShopItem : MonoBehaviour
    {
        [SerializeField] private Image image;
        
        public bool IsOpen { get; set; }
        public ItemInfo ItemInfo { get; set; }
        
        public Action<ShopItem> OnPress;

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