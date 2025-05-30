using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KnifeHit.Scripts
{
    public class InputHandler : MonoBehaviour
    {
        public Action OnClick;
        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                OnClick?.Invoke();
            }
        }
    }
}