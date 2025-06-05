using TriInspector;
using UnityEngine;

namespace KnifeHit.Scripts.GameUi
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