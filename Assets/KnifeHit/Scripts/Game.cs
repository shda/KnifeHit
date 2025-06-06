using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        private bool isCompleteGame;
        private readonly List<Knife> _usedKnifes = new();
        
        private CancellationTokenSource _tokenSource;
        
        public void SetKnifeSpeed(float speed)
        {
            _knifeSpeed = speed;
            UpdateKnife();
        }

        public void SetDelayBetweenKnives(float delayBetweenKnifes)
        {
            delayNextKnife = delayBetweenKnifes;
        }
        
        public void ResetLevelToDefault()
        {
            _tokenSource?.Cancel();
            _tokenSource = new CancellationTokenSource();
            
            inputHandler.enabled = true;
            
            gameStats.LoadValues();
            target.SetDefaultSize();
            
            foreach (var usedKnife in _usedKnifes.Where(usedKnife => usedKnife))
            {
                DestroyImmediate(usedKnife.gameObject);
            }

            SetKnifeSkin(gameStats.IndexSelectKnife.Value);
            
            gameOverScreen.Hide();
            target.RemoveOldObjects();
            luaScriptLoader.StartLevel();

            isCompleteGame = false;
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
            gameStats.CountCurrentBonuses.Value = 0;
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
                bonus.PlayCompleteAnimation();
                //Destroy(bonus.gameObject);
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
                if (gameStats.CountUserKnives.Value <= 0)
                {
                    OnceCompleteGame(false);
                }
            }
            
            var otherKnife = collision.gameObject.GetComponent<Knife>();
            if (otherKnife)
            {
                knife.IsMoving = false;
                knife.PlayCompleteAnimation();
                OnceCompleteGame(true);
            }
        }

        private void OnceCompleteGame(bool isGameOver)
        {
            if (isCompleteGame)
                return;
            
            Debug.Log($"OnceCompleteGame {isGameOver}");
            
            isCompleteGame = true;
            
            if (isGameOver)
            {
                DelayShowGameOverScreen();
            }
            else
            {
                DelayedRestart(2 ,_tokenSource.Token);
            }
        }
        
        private async void DelayShowGameOverScreen()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            ShowGameOverScreen();
        }

        private void OnClick()
        {
            if(isCompleteGame)
                return;
            
            inputHandler.enabled = false;
            
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
                DelayedCreateKnife(delayNextKnife , _tokenSource.Token);
            }
        }
        
        private async void DelayedRestart(float delay , CancellationToken token)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.5), cancellationToken: token);
            if(token.IsCancellationRequested)
                return;
            
            luaScriptLoader.StopLevel(); 
            target.AnimationEndLevelAsync();
            
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            if(token.IsCancellationRequested)
                return;
            
            ResetLevelToDefault();
        }

        private async void DelayedCreateKnife(float delay , CancellationToken token)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: token);
            if(token.IsCancellationRequested)
                return;
            
            PrepareNewKnife();
            inputHandler.enabled = true;
        }

        public void SetKnifeSkin(int skinIndex)
        {
            _skinIndex = skinIndex;
            UpdateKnife();
        }

        private void UpdateKnife()
        {
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

        private void OnDestroy()
        {
            _tokenSource?.Cancel();
        }
    }
}
