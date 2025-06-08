using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using KnifeHit.Scripts.Bonuses;
using KnifeHit.Scripts.LuaLogic;
using UnityEngine;

namespace KnifeHit.Scripts
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private float delayNextKnife;

        [Space] [SerializeField] private InputHandler inputHandler;
        [SerializeField] private Target target;
        [SerializeField] private GameOverScreen gameOverScreen;
        [SerializeField] private LuaScriptLoader luaScriptLoader;
        [SerializeField] private GameStats gameStats;
        [SerializeField] private KnifeSpawner knifeSpawner;

        private bool _isCompleteGame;
        private CancellationTokenSource _tokenSource;

        private void Awake()
        {
            knifeSpawner.OnKnifeCollisionToOther = KnifeCollisionToOther;
            knifeSpawner.OnKnifeTriggerToOther = KnifeTriggerToOther;
        }

        private void Start()
        {
            inputHandler.OnClick = OnClick;
            ResetLevelToDefault();
        }

        public void SetKnifeSpeed(float speed)
        {
            knifeSpawner.KnifeSpeed = speed;
        }

        public void SetDelayBetweenKnives(float delayBetweenKnifes)
        {
            delayNextKnife = delayBetweenKnifes;
        }

        public void ResetLevelToDefault()
        {
            _tokenSource?.Cancel();
            _tokenSource = new CancellationTokenSource();
            
            knifeSpawner.RemoveCurrentKnife();
            target.RemoveOldObjects();

            inputHandler.enabled = true;

            gameStats.LoadValues();
            target.SetDefaultSize();
            SetKnifeSkin(gameStats.IndexSelectKnife.Value);
            gameOverScreen.Hide();
            luaScriptLoader.StartLevel();

            _isCompleteGame = false;
        }

        private void ShowGameOverScreen()
        {
            gameStats.CountCurrentBonuses.Value = 0;
            gameStats.SaveValues();

            gameOverScreen.OnRestartGame = ResetLevelToDefault;
            gameOverScreen.Show();
        }

        private void KnifeTriggerToOther(Knife knife, Collider2D coll)
        {
            var bonus = coll.GetComponent<Bonus>();
            if (bonus)
            {
                AddBonus();
                bonus.PlayCompleteAnimation();
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

        private void KnifeCollisionToOther(Knife knife, Collision2D collision)
        {
            var collisionTarget = collision.gameObject.GetComponent<Target>();
            if (collisionTarget)
            {
                collisionTarget.KnifeHitToTarget(knife);
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
            if (_isCompleteGame)
                return;

            Debug.Log($"OnceCompleteGame {isGameOver}");

            _isCompleteGame = true;

            if (isGameOver)
            {
                DelayShowGameOverScreen();
            }
            else
            {
                DelayedRestart(2, _tokenSource.Token);
            }
        }

        private async void DelayShowGameOverScreen()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            ShowGameOverScreen();
        }

        private void OnClick()
        {
            if (_isCompleteGame)
                return;

            inputHandler.enabled = false;
            var knife = knifeSpawner.ThrowKnife();
            target.AddKnife(knife);

            gameStats.CountUserKnives.Value--;
            if (gameStats.CountUserKnives.Value > 0)
            {
                DelayedCreateKnife(delayNextKnife, _tokenSource.Token);
            }
        }

        private async void DelayedRestart(float delay, CancellationToken token)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.5), cancellationToken: token);
            if (token.IsCancellationRequested)
                return;

            luaScriptLoader.StopLevel();
            target.AnimationEndLevelAsync();

            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            if (token.IsCancellationRequested)
                return;

            ResetLevelToDefault();
        }

        private async void DelayedCreateKnife(float delay, CancellationToken token)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: token);
            if (token.IsCancellationRequested)
                return;

            knifeSpawner.PrepareNewKnife();
            inputHandler.enabled = true;
        }

        public void SetKnifeSkin(int skinIndex)
        {
            knifeSpawner.SkinIndex = skinIndex;
            knifeSpawner.UpdateKnife();
        }

        public void SetCountAllKnives(int all, int current)
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