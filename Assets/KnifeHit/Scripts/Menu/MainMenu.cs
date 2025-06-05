using System;
using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KnifeHit.Scripts.Menu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private SceneReference gameScene;
        [SerializeField] private StartMenu startMenu;
        [SerializeField] private SelectKnifeMenu selectKnifeMenu;

        private void Awake()
        {
            OnPressButtonHome();
        }


        public void OnPressButtonPlay()
        {
            SceneManager.LoadScene(gameScene.Name);
        }

        public void OnPressButtonOperSelectKnife()
        {
            startMenu.gameObject.SetActive(false);
            selectKnifeMenu.gameObject.SetActive(true);
        }

        public void OnPressButtonHome()
        {
            startMenu.gameObject.SetActive(true);
            selectKnifeMenu.gameObject.SetActive(false);
        }
    }
}
