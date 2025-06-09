using System;
using UnityEngine;

namespace KnifeHit.Scripts
{
    public class GameOverScreen : MonoBehaviour
    {
        public Action OnRestartGame;
        public Action OnQuitGame;

        public void OnPressOnQuitGame()
        {
            OnQuitGame?.Invoke();
        }
        
        public void OnPressRestart()
        {
            OnRestartGame?.Invoke();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}