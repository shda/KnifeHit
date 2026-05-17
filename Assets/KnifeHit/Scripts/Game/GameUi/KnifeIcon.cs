using UnityEngine;

namespace KnifeHit.Scripts.Game.GameUi
{
    public class KnifeIcon : MonoBehaviour
    {
        [SerializeField] private Transform enableKnife;
        
        public void SetShowKnife(bool isShow)
        {
            enableKnife.gameObject.SetActive(isShow);
        }
    }
}