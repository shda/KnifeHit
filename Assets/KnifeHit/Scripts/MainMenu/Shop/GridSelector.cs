using UnityEngine;

namespace KnifeHit.Scripts.MainMenu.Shop
{
    public class GridSelector : MonoBehaviour
    {
        [SerializeField] private RectTransform rootRect;
        [SerializeField] private float speed = 1.0f;// Объект, который перемещаем
        
        private RectTransform _targetRect;
        
        public void SetTargetRect(RectTransform targetRect)
        {
            _targetRect = targetRect;
        }
        
        private void Update()
        {
            if(!_targetRect)
                return;
            
            var screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, _targetRect.position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rootRect.parent as RectTransform,
                screenPoint,
                Camera.main,
                out Vector2 localPoint
            );
            rootRect.anchoredPosition = Vector2.Lerp(rootRect.anchoredPosition, localPoint, speed);
        }
    }
}