using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine;

namespace KnifeHit.Scripts
{
    public class GameStats
    {
        public IntReactiveProperty CountCurrentBonuses { get;set; } = new();
        public IntReactiveProperty CurrentLevel { get;set; } = new();
        public IntReactiveProperty CountUserKnives { get;set; } = new();
        public IntReactiveProperty CountAllUserKnives { get;set; } = new();
        public IntReactiveProperty LastOpenedLevel { get;set; } = new();
        public ReactiveProperty<HashSet<int>> OpenedShopItems { get;set; } = new();
        public IntReactiveProperty IndexSelectKnife { get;set; } = new();
        
        public BoolReactiveProperty IsGameOver { get; set; } = new();
        
        public BoolReactiveProperty IsCompletedGame { get; set; } = new();
        
        public CancellationTokenSource TokenSource { get; private set; } = new();

        public void Initialize()
        {
            TokenSource?.Cancel();
            TokenSource = new CancellationTokenSource();
        }
        
        public void Dispose()
        {
            TokenSource?.Cancel();
            TokenSource?.Dispose();
            TokenSource = null;
        }
        
        private void ParsingBoughtItems()
        {
           var openedItemsStr = PlayerPrefs.GetString(nameof(OpenedShopItems));
           if (string.IsNullOrEmpty(openedItemsStr))
           {
               OpenedShopItems.Value = new HashSet<int>();
               return;
           }
           
           // Парсинг строки "0,2,3,4,5,6"
           var openedItemsArr = openedItemsStr.Split(',');
           var openedItems = new int[openedItemsArr.Length];
           for (var i = 0; i < openedItemsArr.Length; i++)
           {
               openedItems[i] = int.Parse(openedItemsArr[i]);
           }
           OpenedShopItems.Value = new HashSet<int>(openedItems);
        }
        
        public void LoadValues()
        {
            CountCurrentBonuses.Value = PlayerPrefs.GetInt(nameof(CountCurrentBonuses));
            LastOpenedLevel.Value = PlayerPrefs.GetInt(nameof(LastOpenedLevel));
            IndexSelectKnife.Value = PlayerPrefs.GetInt(nameof(IndexSelectKnife));
            
            ParsingBoughtItems();
        }

        public void SaveValues()
        {
            PlayerPrefs.SetInt(nameof(CountCurrentBonuses), CountCurrentBonuses.Value);
            PlayerPrefs.SetInt(nameof(LastOpenedLevel), LastOpenedLevel.Value);
            PlayerPrefs.SetInt(nameof(IndexSelectKnife), IndexSelectKnife.Value);
        }
    }
}