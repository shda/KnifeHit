using TMPro;
using UniRx;
using UnityEngine;

namespace KnifeHit.Scripts.GameUi
{
    public class GameSceneUi : MonoBehaviour
    {
        [SerializeField] private PanelIconsCountUserKnifes panelIconsCountUserKnifes;
        [SerializeField] private GameSessionInfo gameSessionInfo;
        [SerializeField] private TextMeshProUGUI countTopBonuses;
        [SerializeField] private TextMeshProUGUI countCurrentBonuses;

        private void Awake()
        {
            gameSessionInfo.CountTopBonuses.Subscribe(i =>
            {
                countTopBonuses.text = i.ToString();
            }).AddTo(this);
            
            gameSessionInfo.CountCurrentBonuses.Subscribe(i =>
            {
                countCurrentBonuses.text = i.ToString();
            }).AddTo(this);;
            
            gameSessionInfo.CountUserKnives.Subscribe(OnChangeCountUserKnives).AddTo(this);
            gameSessionInfo.CountAllUserKnives.Subscribe(OnChangeCountUserKnives).AddTo(this);
        }

        private void OnChangeCountUserKnives(int count)
        {
            panelIconsCountUserKnifes.SetCountKnifes(
                gameSessionInfo.CountAllUserKnives.Value , 
                gameSessionInfo.CountUserKnives.Value);
        }
    }
}