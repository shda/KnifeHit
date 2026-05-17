using System;
using KnifeHit.Scripts.LuaLogic;

namespace KnifeHit.Scripts.Game
{
    public class StartLevel : IDisposable
    {
        private readonly GameStats _gameStats;
        private readonly Target _target;
        private readonly GameOverScreen _gameOverScreen;
        private readonly LuaScriptLoader _luaScriptLoader;
        private readonly KnifeSpawner _knifeSpawner;
        private readonly InputHandler _inputHandler;
        
        public StartLevel(
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
            InitGameStats();

            _knifeSpawner.SkinIndex = _gameStats.IndexSelectKnife.Value;
            _knifeSpawner.UpdateCurrentKnife();
            
            _target.RemoveOldObjects();
            _inputHandler.IsEnable = true;
            
            _target.SetDefaultSize();
            
            _gameOverScreen.Hide();
            _luaScriptLoader.StartLevel();
        }
        private void InitGameStats()
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
            
            
    
        }
        public void Dispose()
        {
            _gameStats.Dispose();
        }
    }
}