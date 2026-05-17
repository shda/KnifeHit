using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using KnifeHit.Scripts.LuaLogic;
using Zenject;

namespace KnifeHit.Scripts
{
    public class KnifeThrowerHandler : IInitializable
    {
        private readonly KnifeSpawner _knifeSpawner;
        private readonly InputHandler _inputHandler;
        private readonly GameStats _gameStats;
        private readonly Target _target;
        private readonly GameSettings _gameSettings;
        public KnifeThrowerHandler(
            KnifeSpawner knifeSpawner, 
            InputHandler inputHandler,
            GameStats gameStats,
            Target target,
            GameSettings gameSettings)
        {
            _knifeSpawner = knifeSpawner;
            _inputHandler = inputHandler;
            _gameStats = gameStats;
            _target = target;
            _gameSettings = gameSettings;
        }
        
        public void Initialize()
        {
            _inputHandler.OnClick += ThrowKnife;
        }

        private void ThrowKnife()
        {
            if(_gameStats.IsCompletedGame.Value)
                return;

            _inputHandler.IsEnable = false;
            
            var newKnife = _knifeSpawner.ThrowKnife();
            _target.AddKnifeToListObjects(newKnife);
            
            _gameStats.CountUserKnives.Value--;
            if (_gameStats.CountUserKnives.Value > 0)
            {
                DelayedCreateKnife(_gameSettings.DelayNextKnife, _gameStats.TokenSource.Token);
            }
        }
        
        private async void DelayedCreateKnife(float delay, CancellationToken token)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: token);
            if (token.IsCancellationRequested)
                return;

            _knifeSpawner.PrepareNewKnife();
            _inputHandler.IsEnable = true;
        }
    }
}