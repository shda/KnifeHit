using Common.Scripts.Extensions;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace KnifeHit.Scripts.MainMenu.Shop
{
    public class ShopMenu : MonoBehaviour
    {
        [SerializeField] private ShopGridItems shopGridItems;
        [SerializeField] private Image previewImage;
        
        [SerializeField] private Transform labelAlreadySelect;
        [SerializeField] private ImageText priceBuy;
        [SerializeField] private ImageText openLevel;
        [SerializeField] private Button buttonBuy;
        [SerializeField] private Button buttonSelect;
        
        private ShopItem _selectItem;
        private GameStats _gameStats;
        private ItemInfo SelectItemInfo => _selectItem.ItemInfo;

        [Inject]
        public void Construct(GameStats gameStats)
        {
            _gameStats = gameStats;
        }
        
        
        private void Start()
        {
            shopGridItems.OnPressShopItem = OnPressShopItem;
            _gameStats.LoadValues();
        }

        private void OnPressShopItem(ShopItem item)
        {
            _selectItem = item;
            previewImage.sprite = SelectItemInfo.KnifeSprite;

            if (SelectItemInfo.IsCanUsing(_gameStats))
            {
                HideAllLabels();
                if (SelectItemInfo.IsSelect(_gameStats))
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
                if (SelectItemInfo.TryGetLevelOpen(out var levelOpen))
                {
                    openLevel.SetValue(levelOpen);
                }
                else if (SelectItemInfo.TryGetPrice(out var price))
                {
                    priceBuy.SetValue(price);
                    buttonBuy.gameObject.SetActive(true);

                    buttonBuy.interactable = price <= _gameStats.CountCurrentBonuses.Value;
                }
            }
        }

        private void HideAllLabels()
        {
            priceBuy.Hide();
            openLevel.Hide();
            labelAlreadySelect.Hide();
            buttonBuy.Hide();
            buttonSelect.Hide();
        }

        public void OnPressBuy()
        {
            if (SelectItemInfo.IsCanBuy(_gameStats))
            {
                SelectItemInfo.Buy(_gameStats);
                _selectItem.UpdateSprite();
                OnPressSelect();
            }
        }

        public void OnPressSelect()
        {
            SelectItemInfo.SelectItem(_gameStats);
            _selectItem.UpdateSprite();
            OnPressShopItem(_selectItem);
        }
    }
}