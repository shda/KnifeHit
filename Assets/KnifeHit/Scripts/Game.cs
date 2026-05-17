using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Eflatun.SceneReference;
using KnifeHit.Scripts.Bonuses;
using KnifeHit.Scripts.LuaLogic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KnifeHit.Scripts
{

    public class Game : MonoBehaviour
    {
        [SerializeField] private float delayNextKnife;

        [Space] [SerializeField] 
        private InputHandler inputHandler;
        [SerializeField] private Target target;
        [SerializeField] private GameOverScreen gameOverScreen;
        [SerializeField] private LuaScriptLoader luaScriptLoader;
        [SerializeField] private GameStats gameStats;
        [SerializeField] private KnifeSpawner knifeSpawner;

        [SerializeField] private SceneReference mainMenuScene;
        private bool _isCompleteGame;
        private bool _isGameOver;
        private CancellationTokenSource _tokenSource;

        
        private void Awake()
        {
            knifeSpawner.OnKnifeCollisionToOther = KnifeCollisionToOther;
            knifeSpawner.OnKnifeTriggerToOther = KnifeTriggerToOther;
            
            inputHandler.OnClick = OnClick;
        }

        private void Start()
        {
            gameStats.CurrentLevel.Value = 0;
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

            gameStats.CurrentLevel.Value++;
            if (gameStats.CurrentLevel.Value > gameStats.LastOpenedLevel.Value)
            {
                gameStats.LastOpenedLevel.Value = gameStats.CurrentLevel.Value; 
                gameStats.SaveValues();
            }
            
            _isGameOver = false;
            
            knifeSpawner.UpdateCurrentKnife();
            target.RemoveOldObjects();
           // inputHandler.enabled = true;

            gameStats.LoadValues();
            target.SetDefaultSize();
            
            SetKnifeSkin(gameStats.IndexSelectKnife.Value);
            
            gameOverScreen.Hide();
            luaScriptLoader.StartLevel();

            _isCompleteGame = false;
        }

        private void ShowGameOverScreen()
        {
            gameStats.SaveValues();

            gameOverScreen.OnRestartGame = ResetLevelToDefault;
            gameOverScreen.OnQuitGame = QuitGame;
            gameOverScreen.Show();
        }

        private void QuitGame()
        {
            SceneManager.LoadScene(mainMenuScene.Name);
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
            gameStats.SaveValues();
        }

        private void KnifeCollisionToOther(Knife knife, Collision2D collision)
        {
            var collisionTarget = collision.gameObject.GetComponent<Target>();
            if (collisionTarget)
            {
                collisionTarget.KnifeHitToTarget(knife);
                if (gameStats.CountUserKnives.Value <= 0)
                {
                    OnceCompleteGame();
                }
            }

            var otherKnife = collision.gameObject.GetComponent<Knife>();
            if (otherKnife)
            {
                knife.IsMoving = false;
                knife.PlayCompleteAnimation();

                _isGameOver = true;
                OnceCompleteGame();
            }
        }

        private void OnceCompleteGame()
        {
            if (_isCompleteGame)
                return;

            Debug.Log($"OnceCompleteGame {_isGameOver}");
            _isCompleteGame = true;
            
            DelayCompleteGameAsync();
        }

        private async void DelayCompleteGameAsync()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.5));
            
            if (_isGameOver)
            {
                ShowGameOverScreen();
            }
            else
            {
                DelayedRestart(2, _tokenSource.Token);
            }
        }
        
        private async void DelayedRestart(float delay, CancellationToken token)
        {
            luaScriptLoader.StopLevel();
            target.AnimationEndLevelAsync();

            await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: token);
            if (token.IsCancellationRequested)
                return;

            ResetLevelToDefault();
        }
        
        private void OnClick()
        {
            if (_isCompleteGame)
                return;

           // inputHandler.enabled = false;
            var newKnife = knifeSpawner.ThrowKnife();
            target.AddKnifeToListObjects(newKnife);

            gameStats.CountUserKnives.Value--;
            if (gameStats.CountUserKnives.Value > 0)
            {
                DelayedCreateKnife(delayNextKnife, _tokenSource.Token);
            }
        }

        

        private async void DelayedCreateKnife(float delay, CancellationToken token)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: token);
            if (token.IsCancellationRequested)
                return;

            knifeSpawner.PrepareNewKnife();
            //inputHandler.enabled = true;
        }

        public void SetKnifeSkin(int skinIndex)
        {
            knifeSpawner.SkinIndex = skinIndex;
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