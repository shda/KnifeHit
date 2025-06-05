using TMPro;
using UniRx;
using UnityEngine;

namespace KnifeHit.Scripts.GameUi
{
    public class CountTopBonuses : MonoBehaviour
    {
        [SerializeField] private GameSessionInfo gameSessionInfo;
        [SerializeField] private TextMeshProUGUI countTopBonuses;

        private void Awake()
        {
            gameSessionInfo.LoadValues();
            gameSessionInfo.CountTopBonuses.Subscribe(i =>
            {
                countTopBonuses.text = i.ToString();
            }).AddTo(this);
        }
    }
}