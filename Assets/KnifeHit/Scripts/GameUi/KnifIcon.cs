using TriInspector;
using UnityEngine;

namespace KnifeHit.Scripts.GameUi
{
    public class KnifIcon : MonoBehaviour
    {
        [SerializeField] private Transform enableKnife;
        
        public void SwitchShowKnife(bool isShow)
        {
            enableKnife.gameObject.SetActive(isShow);
        }
    }
}