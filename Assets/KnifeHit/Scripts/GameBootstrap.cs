using System;
using KnifeHit.Scripts.LuaLogic;

namespace KnifeHit.Scripts
{
    public class GameBootstrap : IDisposable
    {
        private readonly GameStats _gameStats;
        private readonly Target _target;
        private readonly GameOverScreen _gameOverScreen;
        private readonly LuaScriptLoader _luaScriptLoader;
        private readonly KnifeSpawner _knifeSpawner;
        private readonly InputHandler _inputHandler;
        
        public GameBootstrap(
            GameStats gameStats,  
            Target target, 
            KnifeSpawner knifeSpawner,
            InputHandler inputHandler,
            GameOverScreen gameOverScreen,
            LuaScriptLoader luaScriptLoader)
        {
            _gameStats = gameStats;
            _target = target;
            _knifeSpawner = knifeSpawner;
            _inputHandler = inputHandler;
            _gameOverScreen = gameOverScreen;
            _luaScriptLoader = luaScriptLoader;
        }
        
        public void StartGame()
        {
            _gameStats.CurrentLevel.Value = 0;
            ResetLevelToDefault();
        }
        
        public void ResetLevelToDefault()
        {
            _gameStats.Initialize();
            _gameStats.CurrentLevel.Value++;
            if (_gameStats.CurrentLevel.Value > _gameStats.LastOpenedLevel.Value)
            {
                _gameStats.LastOpenedLevel.Value = _gameStats.CurrentLevel.Value; 
                _gameStats.SaveValues();
            }
            
            _gameStats.LoadValues();

            _gameStats.IsGameOver.Value = false;
            _gameStats.IsCompletedGame.Value = false;

            _knifeSpawner.UpdateCurrentKnife();
            _target.RemoveOldObjects();
            _inputHandler.IsEnable = true;

            //gameStats.LoadValues();
            _target.SetDefaultSize();

           // SetKnifeSkin(gameStats.IndexSelectKnife.Value);

            _gameOverScreen.Hide();
            _luaScriptLoader.StartLevel();

           // _isCompleteGame = false;
        }
        public void Dispose()
        {
            _gameStats.Dispose();
        }
    }
}