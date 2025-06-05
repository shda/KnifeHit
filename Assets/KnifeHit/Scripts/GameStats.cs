using UniRx;
using UnityEngine;

namespace KnifeHit.Scripts
{
    [CreateAssetMenu(menuName = "Create GameStats", fileName = "GameStats", order = 0)]
    public class GameStats : ScriptableObject
    {
        public IntReactiveProperty CountTopBonuses { get;set; } = new();
        public IntReactiveProperty CountCurrentBonuses { get;set; } = new();
        public IntReactiveProperty CountUserKnives { get;set; } = new();
        public IntReactiveProperty CountAllUserKnives { get;set; } = new();
        
        public void LoadValues()
        {
            CountCurrentBonuses.Value = 0;
            CountTopBonuses.Value = PlayerPrefs.GetInt(nameof(CountTopBonuses));
        }

        public void SaveValues()
        {
            PlayerPrefs.SetInt(nameof(CountTopBonuses), CountTopBonuses.Value);
        }
    }
}