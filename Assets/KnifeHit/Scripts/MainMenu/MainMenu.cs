using Eflatun.SceneReference;
using KnifeHit.Scripts.MainMenu.Shop;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KnifeHit.Scripts.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private SceneReference gameScene;
        [SerializeField] private WelcomeMenu welcomeMenu;
        [SerializeField] private ShopMenu shopMenu;

        private void Awake()
        {
            OnPressButtonHome();
        }
        
        public void OnPressButtonPlay()
        {
            SceneManager.LoadScene(gameScene.Name);
        }

        public void OnPressButtonOpenSelectKnife()
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
