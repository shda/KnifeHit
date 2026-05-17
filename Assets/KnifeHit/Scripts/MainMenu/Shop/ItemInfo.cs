using System;
using UnityEngine;

namespace KnifeHit.Scripts.MainMenu.Shop
{
    [Serializable]
    public class ItemInfo
    {
        [Space]
        [SerializeField] private Sprite knifeSprite;
        [SerializeField] private Sprite shadowKnifeSprite;
        [SerializeField] private int indexImage;
        [SerializeField] private bool alwaysOpen;
        [SerializeField] private int buyPrice = -1;
        [SerializeField] private int needOpenLevel = -1;
        
        public Sprite KnifeSprite => knifeSprite;
        public Sprite ShadowKnifeSprite => shadowKnifeSprite;
        
        public bool IsCanUsing(GameStats gameStats)
        {
            if (alwaysOpen)
                return true;
            
            if (IsBayed(gameStats))
                return true;
            
            if (needOpenLevel != -1)
            {
                return gameStats.LastOpenedLevel.Value >= needOpenLevel;
            }
            
            return false;
        }
        private bool IsBayed(GameStats gameStats)
        {
            if (gameStats.OpenedShopItems.Value.Contains(indexImage))
                return true;
            return false;
        }

        public bool IsCanBuy(GameStats gameStats)
        {
            if (buyPrice != -1)
            {
                return gameStats.CountCurrentBonuses.Value >= buyPrice;
            }
            
            return false;
        }

        public void BuyItem(GameStats gameStats)
        {
            gameStats.OpenedShopItems.Value.Add(indexImage);
            gameStats.SaveValues();
        }

        public bool TryGetPrice(out int price)
        {
            if(buyPrice != -1)
            {
                price = buyPrice;
                return true;
            }

            price = -1;
            return false;
        }
        
        public bool TryGetLevelOpen(out int levelOpen)
        {
            if(needOpenLevel != -1)
            {
                levelOpen = needOpenLevel;
                return true;
            }

            levelOpen = -1;
            return false;
        }

        public bool IsSelect(GameStats gameStats)
        {
            return gameStats.IndexSelectKnife.Value == indexImage;
        }

        public bool Buy(GameStats gameStats)
        {
            if(buyPrice != -1)
            {
                if (gameStats.CountCurrentBonuses.Value >= buyPrice)
                {
                    gameStats.CountCurrentBonuses.Value -= buyPrice;
                    BuyItem(gameStats);
                    return true;
                }
            }

            return false;
        }

        public void SelectItem(GameStats gameStats)
        {
            gameStats.IndexSelectKnife.Value = indexImage;
            gameStats.SaveValues();
        }
    }
}