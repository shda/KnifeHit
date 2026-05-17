using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace KnifeHit.Scripts.Game.GameUi
{
    public class BestScoreLabel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI countTopBonuses;
        
        private GameStats _gameStats;
        
        [Inject]
        public void Construct(GameStats gameStats)
        {
            _gameStats = gameStats;
        }

        private void Start()
        {
            _gameStats.LoadValues();
            _gameStats.CountCurrentBonuses.Subscribe(i =>
            {
                countTopBonuses.text = i.ToString();
            }).AddTo(this);
        }
    }
}