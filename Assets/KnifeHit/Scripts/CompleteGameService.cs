using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using KnifeHit.Scripts.LuaLogic;
using UniRx;
using Zenject;

namespace KnifeHit.Scripts
{
    public class CompleteGameService : IInitializable , IDisposable
    {
        private readonly GameStats _gameStats;
        private readonly GameOverScreen _gameOverScreen;
        private readonly LuaScriptLoader _luaScriptLoader;
        private readonly Target _target;
        private readonly StartLevel _startLevel;
        
        private readonly CompositeDisposable _disposables = new();
        
        public CompleteGameService(
            GameStats gameStats,
            GameOverScreen gameOverScreen,
            LuaScriptLoader luaScriptLoader,
            Target target,
            StartLevel startLevel)
        {
            _gameStats = gameStats;
            _luaScriptLoader = luaScriptLoader;
            _target = target;
            _gameOverScreen = gameOverScreen;
            _startLevel = startLevel;
        }
        public void Initialize()
        {
            _gameStats.IsCompletedGame.Subscribe(OnIsCompletedGame).AddTo(_disposables);
            _gameStats.IsGameOver.Subscribe(OnIsGameOver).AddTo(_disposables);
        }
        private void OnIsGameOver(bool isGameOver)
        {
            if (isGameOver)
            {
                _gameOverScreen.OnRestartGame = ResetLevelToDefault;
                _gameOverScreen.OnQuitGame = QuitGame;
                _gameOverScreen.Show();
            }
        }
        private void QuitGame()
        {
            throw new NotImplementedException();
        }
        private void ResetLevelToDefault()
        {
            _startLevel.ResetLevelToDefault();
        }
        private void OnIsCompletedGame(bool isInCompleteGame)
        {
            if (isInCompleteGame)
            {
                DelayCompleteGameAsync();
            }
        }
        
        private async void DelayCompleteGameAsync()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.5));
            
            if (_gameStats.IsGameOver.Value)
            {
                _gameOverScreen.OnRestartGame = ResetLevelToDefault;
                _gameOverScreen.OnQuitGame = QuitGame;
                _gameOverScreen.Show();
            }
            else
            {
                DelayedRestart(2, _gameStats.TokenSource.Token);
            }
        }
        
        private async void DelayedRestart(float delay, CancellationToken token)
        {
            _luaScriptLoader.StopLevel();
            _target.AnimationEndLevelAsync();

            await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: token);
            if (token.IsCancellationRequested)
                return;

            ResetLevelToDefault();
        }
        
        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}