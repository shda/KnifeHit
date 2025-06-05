using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace KnifeHit.Scripts.GameUi
{
    public class GameSceneUi : MonoBehaviour
    {
        [SerializeField] private AttemptsCounter attemptsCounter;
        [SerializeField] private GameStats gameStats;
        [SerializeField] private TextMeshProUGUI countCurrentBonuses;

        private void Awake()
        {
            gameStats.CountCurrentBonuses.Subscribe(i =>
            {
                countCurrentBonuses.text = i.ToString();
            }).AddTo(this);;
            
            gameStats.CountUserKnives.Subscribe(OnChangeCountUserKnives).AddTo(this);
            gameStats.CountAllUserKnives.Subscribe(OnChangeCountUserKnives).AddTo(this);
        }

        private void OnChangeCountUserKnives(int count)
        {
            attemptsCounter.SetCountKnifes(
                gameStats.CountAllUserKnives.Value , 
                gameStats.CountUserKnives.Value);
        }
    }
}