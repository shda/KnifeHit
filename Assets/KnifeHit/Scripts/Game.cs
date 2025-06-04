using System;
using System.Collections.Generic;
using BlockBlast.Scripts.Common;
using Cysharp.Threading.Tasks;
using KnifeHit.Scripts.Bonuses;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KnifeHit.Scripts
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private float delayNextKnife;
        [SerializeField] private InputHandler inputHandler;
        [SerializeField] private Transform startSpawnKnife;
        [SerializeField] private Target target;
        [SerializeField] private GameOverScreen  gameOverScreen;
        [SerializeField] private PoolGameObjectsComponent<Knife> knifePool;

        private Knife _currentKnife;
        private List<Knife> _usedKnifes;

        private void Awake()
        {
            knifePool.Init();
        }

        private void Start()
        {
            inputHandler.OnClick = OnClick;
            PrepareNewKnife();
        }

        private void PrepareNewKnife()
        {
            _currentKnife = knifePool.Get();
            _currentKnife.SwitchCollider(false);
            //_currentKnife.transform.SetParent(startSpawnKnife);
            _currentKnife.transform.position = startSpawnKnife.position;

            _currentKnife.OnCollision = KnifeCollisionToOther;
            _currentKnife.OnTriggerEnter = KnifeTriggerToOther;
        }

        private void ShowGameOverScreen()
        {
            gameOverScreen.OnRestartGame = () =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            };
            
            gameOverScreen.Show();
        }

        private void KnifeTriggerToOther(Knife knife , Collider2D coll)
        {
            var bonus = coll.GetComponent<BonusBase>();
            if (bonus)
            {
                Destroy(bonus.gameObject);
            }
        }

        private void KnifeCollisionToOther(Knife knife , Collision2D collision)
        {
            var collisionTarget = collision.gameObject.GetComponent<Target>();
            if (collisionTarget)
            {
                knife.IsMoving = false;
                knife.SetStaticRigidbody2D();
                knife.transform.SetParent(target.transform);
                
                return;
            }
            
            var otherKnife = collision.gameObject.GetComponent<Knife>();
            if (otherKnife)
            {
                knife.IsMoving = false;
                ShowGameOverScreen();
            }
        }

        private void OnClick()
        {
            if (_currentKnife)
            {
                _currentKnife.SwitchCollider(true);
                _currentKnife.KnifeThrow();
                _currentKnife = null;
            }
            
            DelayedCreateKnife(delayNextKnife);
        }

        private async void DelayedCreateKnife(float delay)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            PrepareNewKnife();
        }
    }
}
