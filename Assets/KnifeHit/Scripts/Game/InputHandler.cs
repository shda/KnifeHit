using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace KnifeHit.Scripts.Game
{
    public class InputHandler : ITickable
    {
        public Action OnClick { get; set; }
        
        public bool IsEnable { get; set; }
        
        public void Tick()
        {
            if(!IsEnable)
                return;
            
            if (Input.GetMouseButtonDown(0) && 
                !EventSystem.current.IsPointerOverGameObject())
            {
                OnClick?.Invoke();
            }
        }
    }
}