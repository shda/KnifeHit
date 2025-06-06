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
        [Space]
        [SerializeField] private InputHandler inputHandler;
        [SerializeField] private Transform startSpawnKnife;
        [SerializeField] private Target target;
        [SerializeField] private GameOverScreen  gameOverScreen;
        [SerializeField] private LuaScriptLoader luaScriptLoader;
        [SerializeField] private ListKnifes listKnifes;
        [SerializeField] private GameStats gameStats;

        private float _knifeSpeed;
        private int _skinIndex;
        private Knife _currentKnife;
        private readonly List<Knife> _usedKnifes = new();
        
        public void SetKnifeSpeed(float speed)
        {
            _knifeSpeed = speed;
        }

        public void SetDelayBetweenKnives(float delayBetweenKnifes)
        {
            delayNextKnife = delayBetweenKnifes;
        }
        
        public void ResetLevelToDefault()
        {
            inputHandler.enabled = true;
            
            gameStats.LoadValues();
            target.SetDefaultSize();
            
            foreach (var usedKnife in _usedKnifes)
            {
                if(usedKnife)
                    DestroyImmediate(usedKnife.gameObject);
            }

            gameOverScreen.Hide();
            target.RemoveOldObjects();
            luaScriptLoader.StartLevel();
        }
        
        private void Start()
        {
            inputHandler.OnClick = OnClick;
            ResetLevelToDefault();
        }

        private void PrepareNewKnife()
        {
            _currentKnife = Instantiate(listKnifes.GetWithOverflow(_skinIndex));
            _currentKnife.SwitchCollider(false);
            _currentKnife.SetVelocity(_knifeSpeed);

            _currentKnife.transform.position = startSpawnKnife.position;

            _currentKnife.OnCollision = KnifeCollisionToOther;
            _currentKnife.OnTriggerEnter = KnifeTriggerToOther;
        }

        private void ShowGameOverScreen()
        {
            gameStats.SaveValues();
            gameOverScreen.OnRestartGame = () =>
            {
                ResetLevelToDefault();
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
            gameStats.CountCurrentBonuses.Value++;
            if (gameStats.CountCurrentBonuses.Value > gameStats.CountTopBonuses.Value)
            {
                gameStats.CountTopBonuses.Value = gameStats.CountCurrentBonuses.Value;
                gameStats.SaveValues();
            }
        }

        private void KnifeCollisionToOther(Knife knife , Collision2D collision)
        {
            var collisionTarget = collision.gameObject.GetComponent<Target>();
            if (collisionTarget)
            {
                collisionTarget.HitToTarget(knife);
                return;
            }
            
            var otherKnife = collision.gameObject.GetComponent<Knife>();
            if (otherKnife)
            {
                inputHandler.enabled = false;
                knife.IsMoving = false;
                knife.PlayCompleteAnimation();
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

            gameStats.CountUserKnives.Value--;
            if (gameStats.CountUserKnives.Value > 0)
            {
                DelayedCreateKnife(delayNextKnife);
            }
            else
            {
                inputHandler.enabled = false;
                
                /*
                foreach (var usedKnife in _usedKnifes)
                {
                    usedKnife.PlayCompleteAnimation();
                }
                */

                DelayedRestart(2);
                //ShowGameOverScreen();
            }
        }
        
        
        private async void DelayedRestart(float delay)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.5));
            
            luaScriptLoader.StopLevel();
            target.AnimationEndLevelAsync();
            
            
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            ResetLevelToDefault();
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
            gameStats.CountAllUserKnives.SetValueAndForceNotify(all);
            gameStats.CountUserKnives.SetValueAndForceNotify(current);
        }
    }
}
