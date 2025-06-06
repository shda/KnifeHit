using TriInspector;
using UnityEngine;

namespace KnifeHit.Scripts
{
    public class HitTargetAnimation : MonoBehaviour
    {
        [SerializeField] private Vector3 hitOffset;
        [SerializeField] private float durationBackToPositon = 0.5f;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Color hitColor = Color.white;

        private Color _defaultColor;
        private float _currentTime = float.MaxValue;
        private Vector3 _defaultPosition;
        
        private void Awake()
        {
            _defaultPosition =  transform.position;
            _defaultColor = spriteRenderer.color;
        }
        
        [Button]
        public void PlayAnimation()
        {
            transform.position =  _defaultPosition;
            transform.position += hitOffset;
            spriteRenderer.color = _defaultColor;
            _currentTime = 0;
        }
        
        private void Update()
        {
            _currentTime += Time.deltaTime * durationBackToPositon;
            transform.position = Vector3.Lerp(transform.position, _defaultPosition, _currentTime);
            spriteRenderer.color = Color.Lerp(hitColor , _defaultColor , _currentTime);
        }
    }
}