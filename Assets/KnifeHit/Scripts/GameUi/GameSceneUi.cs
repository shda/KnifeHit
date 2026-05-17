using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace KnifeHit.Scripts.GameUi
{
    public class GameSceneUi : MonoBehaviour
    {
        [SerializeField] private AttemptsCounter attemptsCounter;
        [SerializeField] private TextMeshProUGUI numberLevel;
        
        private GameStats _gameStats;

        [Inject]
        public void Construct(GameStats gameStats)
        {
            _gameStats = gameStats;
        }

        private void Awake()
        {
            _gameStats.CurrentLevel.Subscribe(i =>
            {
                numberLevel.text = i.ToString();
            }).AddTo(this);
            
            _gameStats.CountUserKnives.Subscribe(OnChangeCountUserKnives).AddTo(this);
            _gameStats.CountAllUserKnives.Subscribe(OnChangeCountUserKnives).AddTo(this);
        }

        private void OnChangeCountUserKnives(int count)
        {
            attemptsCounter.SetCountKnifes(
                _gameStats.CountAllUserKnives.Value , 
                _gameStats.CountUserKnives.Value);
        }
    }
}