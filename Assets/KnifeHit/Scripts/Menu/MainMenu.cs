using System;
using Eflatun.SceneReference;
using KnifeHit.Scripts.Menu.Shop;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KnifeHit.Scripts.Menu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private SceneReference gameScene;
        [SerializeField] private WelcomeMenu welcomeMenu;
        [SerializeField] private ShopMenu shopMenu;

        private void Awake()
        {
            //OnPressButtonHome();
        }
        
        public void OnPressButtonPlay()
        {
            SceneManager.LoadScene(gameScene.Name);
        }

        public void OnPressButtonOperSelectKnife()
        {
            welcomeMenu.gameObject.SetActive(false);
            shopMenu.gameObject.SetActive(true);
        }

        public void OnPressButtonHome()
        {
            welcomeMenu.gameObject.SetActive(true);
            shopMenu.gameObject.SetActive(false);
        }
    }
}
