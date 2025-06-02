using System;
using System.Collections.Generic;
using BlockBlast.Scripts.Common;
using BlockBlast.Scripts.Game;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KnifeHit.Scripts
{
    public class Game : MonoBehaviour
    {
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
            _currentKnife.transform.SetParent(startSpawnKnife);

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

        private void KnifeTriggerToOther(Collider2D coll)
        {
            var bonus = coll.GetComponent<BonusBase>();
            if (bonus)
            {
                Destroy(bonus.gameObject);
            }
        }

        private void KnifeCollisionToOther(Collision2D collision)
        {
            var collisionTarget = collision.gameObject.GetComponent<Target>();
            if (collisionTarget)
            {
                _currentKnife.IsMoving = false;
                _currentKnife.SetStaticRigidbody2D();
                _currentKnife.transform.SetParent(target.transform);

                PrepareNewKnife();
                
                return;
            }
            
            var otherKnife = collision.gameObject.GetComponent<Knife>();
            if (otherKnife)
            {
                _currentKnife.IsMoving = false;
                ShowGameOverScreen();
            }
        }

        private void OnClick()
        {
            _currentKnife.KnifeThrow();
        }
    }
}
