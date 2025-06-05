using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using KnifeHit.Scripts.Bonuses;
using KnifeHit.Scripts.Lists;
using KnifeHit.Scripts.LuaLogic;
using UnityEngine;

namespace KnifeHit.Scripts
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private float delayNextKnife;
        [SerializeField] private InputHandler inputHandler;
        [SerializeField] private Transform startSpawnKnife;
        [SerializeField] private Target target;
        [SerializeField] private GameOverScreen  gameOverScreen;
        [SerializeField] private LuaLoaderLogic luaLoaderLogic;
        [SerializeField] private ListKnifes listKnifes;
        
        [SerializeField] private GameSessionInfo gameSessionInfo;
        
        private int _skinIndex;
        private Knife _currentKnife;
        private readonly List<Knife> _usedKnifes = new();

        public void Restart()
        {
            gameSessionInfo.LoadValues();
            
            foreach (var usedKnife in _usedKnifes)
            {
                if(usedKnife)
                    DestroyImmediate(usedKnife.gameObject);
            }

            gameOverScreen.Hide();
            target.RemoveOldObjects();
            luaLoaderLogic.StartLevel();
        }
        
        private void Start()
        {
            inputHandler.OnClick = OnClick;
            Restart();
        }

        private void PrepareNewKnife()
        {
            _currentKnife = Instantiate(listKnifes.GetWithOverflow(_skinIndex));
            _currentKnife.SwitchCollider(false);

            _currentKnife.transform.position = startSpawnKnife.position;

            _currentKnife.OnCollision = KnifeCollisionToOther;
            _currentKnife.OnTriggerEnter = KnifeTriggerToOther;
        }

        private void ShowGameOverScreen()
        {
            gameSessionInfo.SaveValues();
            gameOverScreen.OnRestartGame = () =>
            {
                Restart();
            };
            
            gameOverScreen.Show();
        }

        private void KnifeTriggerToOther(Knife knife , Collider2D coll)
        {
            var bonus = coll.GetComponent<BonusBase>();
            if (bonus)
            {
                AddBonus();
                Destroy(bonus.gameObject);
            }
        }

        private void AddBonus()
        {
            gameSessionInfo.CountCurrentBonuses.Value++;
            if (gameSessionInfo.CountCurrentBonuses.Value > gameSessionInfo.CountTopBonuses.Value)
            {
                gameSessionInfo.CountTopBonuses.Value = gameSessionInfo.CountCurrentBonuses.Value;
                gameSessionInfo.SaveValues();
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
                _usedKnifes.Add(_currentKnife);
                _currentKnife = null;
            }

            gameSessionInfo.CountUserKnives.Value--;
            if (gameSessionInfo.CountUserKnives.Value > 0)
            {
                DelayedCreateKnife(delayNextKnife);
            }
        }

        private async void DelayedCreateKnife(float delay)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            PrepareNewKnife();
        }

        public void SetKnifeSkin(int skinIndex)
        {
            _skinIndex = skinIndex;
            if (_currentKnife)
            {
                DestroyImmediate(_currentKnife.gameObject);
            }
            
            PrepareNewKnife();
        }
        
        public void SetCountAllKnives(int all , int current)
        {
            gameSessionInfo.CountAllUserKnives.SetValueAndForceNotify(all);
            gameSessionInfo.CountUserKnives.SetValueAndForceNotify(current);
        }
    }
}
