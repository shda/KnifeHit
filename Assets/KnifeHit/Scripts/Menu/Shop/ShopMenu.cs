using UnityEngine;
using UnityEngine.UI;

namespace KnifeHit.Scripts.Menu.Shop
{
    public class ShopMenu : MonoBehaviour
    {
        [SerializeField] private ShopGridItems shopGridItems;
        [SerializeField] private Image previewImage;
        
        [SerializeField] private GameStats gameStats;
        
        [SerializeField] private Transform labelAlreadySelect;
        [SerializeField] private ImageText priceBuy;
        [SerializeField] private ImageText openLevel;
        [SerializeField] private Button buttonBuy;
        [SerializeField] private Button buttonSelect;

        private ShopItem _selectItem;
        private ItemInfo _selectItemInfo => _selectItem.ItemInfo;
        
        private void Awake()
        {
            shopGridItems.OnPressShopItem = OnPressShopItem;
            gameStats.LoadValues();
        }

        private void OnPressShopItem(ShopItem item)
        {
            _selectItem = item;
            previewImage.sprite = _selectItemInfo.KnifeSprite;

            if (_selectItemInfo.IsCanUsing(gameStats))
            {
                HideAllLabels();
                if (_selectItemInfo.IsSelect(gameStats))
                {
                    labelAlreadySelect.gameObject.SetActive(true);
                }
                else
                {
                    buttonSelect.gameObject.SetActive(true);
                }
            }
            else
            {
                HideAllLabels();
                if (_selectItemInfo.TryGetLevelOpen(out var levelOpen))
                {
                    openLevel.SetValue(levelOpen);
                }
                else if (_selectItemInfo.TryGetPrice(out var price))
                {
                    priceBuy.SetValue(price);
                    buttonBuy.gameObject.SetActive(true);

                    buttonBuy.interactable = price <= gameStats.CountTopBonuses.Value;
                }
            }
        }

        private void HideAllLabels()
        {
            priceBuy.gameObject.SetActive(false);
            openLevel.gameObject.SetActive(false);
            labelAlreadySelect.gameObject.SetActive(false);
            buttonBuy.gameObject.SetActive(false);
            buttonSelect.gameObject.SetActive(false);
        }

        public void OnPressBuy()
        {
            if (_selectItemInfo.IsCanBuy(gameStats))
            {
                _selectItemInfo.Buy(gameStats);
                _selectItem.UpdateSprite();
                OnPressSelect();
                //OnPressShopItem(_selectItem);
            }
        }

        public void OnPressSelect()
        {
            _selectItemInfo.SelectItem(gameStats);
            _selectItem.UpdateSprite();
            OnPressShopItem(_selectItem);
        }
    }
}