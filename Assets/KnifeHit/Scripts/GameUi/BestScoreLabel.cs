using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace KnifeHit.Scripts.GameUi
{
    public class BestScoreLabel : MonoBehaviour
    {
        [SerializeField] private GameStats gameStats;
        [SerializeField] private TextMeshProUGUI countTopBonuses;

        private void Awake()
        {
            gameStats.LoadValues();
            gameStats.CountTopBonuses.Subscribe(i =>
            {
                countTopBonuses.text = i.ToString();
            }).AddTo(this);
        }
    }
}