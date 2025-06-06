using System;
using UnityEngine;
using UnityEngine.UI;

namespace KnifeHit.Scripts.Menu.Shop
{
    public class ShopItem : MonoBehaviour
    {
        [SerializeField] private Image image;

        private GameStats _gameStats;
        public ItemInfo ItemInfo { get; private set; }
        
        public Action<ShopItem> OnPress;

        public void OnPressItem()
        {
            OnPress?.Invoke(this);
        }
        
        public void SetInfo(ItemInfo itemInfo, GameStats gameStats)
        {
            ItemInfo = itemInfo;
            image.sprite = itemInfo.ShadowKnifeSprite;
            _gameStats = gameStats;

            UpdateSprite();
        }

        public void UpdateSprite()
        {
            image.sprite = ItemInfo.IsCanUsing(_gameStats) ? ItemInfo.KnifeSprite : ItemInfo.ShadowKnifeSprite;
        }
    }
}