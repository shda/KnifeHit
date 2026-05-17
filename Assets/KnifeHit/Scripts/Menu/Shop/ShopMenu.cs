using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace KnifeHit.Scripts.Menu.Shop
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
        private ItemInfo _selectItemInfo => _selectItem.ItemInfo;

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
            previewImage.sprite = _selectItemInfo.KnifeSprite;

            if (_selectItemInfo.IsCanUsing(_gameStats))
            {
                HideAllLabels();
                if (_selectItemInfo.IsSelect(_gameStats))
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

                    buttonBuy.interactable = price <= _gameStats.CountCurrentBonuses.Value;
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
            if (_selectItemInfo.IsCanBuy(_gameStats))
            {
                _selectItemInfo.Buy(_gameStats);
                _selectItem.UpdateSprite();
                OnPressSelect();
                //OnPressShopItem(_selectItem);
            }
        }

        public void OnPressSelect()
        {
            _selectItemInfo.SelectItem(_gameStats);
            _selectItem.UpdateSprite();
            OnPressShopItem(_selectItem);
        }
    }
}