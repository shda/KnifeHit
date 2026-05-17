using KnifeHit.Scripts.Bonuses;
using UnityEngine;
using Zenject;

namespace KnifeHit.Scripts
{
    public class KnifeHitService : IInitializable
    {
        private readonly KnifeSpawner _knifeSpawner;
        private readonly GameStats _gameStats;

        public KnifeHitService(KnifeSpawner knifeSpawner , GameStats gameStats)
        {
            _knifeSpawner = knifeSpawner;
            _gameStats = gameStats;
        }

        public void Initialize()
        {
            _knifeSpawner.OnKnifeCollisionToOther = KnifeCollisionToOther;
            _knifeSpawner.OnKnifeTriggerToOther = KnifeTriggerToOther;
        }
        
        private void KnifeCollisionToOther(Knife knife, Collision2D collision)
        {
            var collisionTarget = collision.gameObject.GetComponent<Target>();
            if (collisionTarget)
            {
                collisionTarget.KnifeHitToTarget(knife);
                if (_gameStats.CountUserKnives.Value <= 0)
                {
                    OnceCompleteGame();
                }
            }

            var otherKnife = collision.gameObject.GetComponent<Knife>();
            if (otherKnife)
            {
                knife.IsMoving = false;
                knife.PlayCompleteAnimation();

                _gameStats.IsGameOver.Value = true;
                OnceCompleteGame();
            }
        }
        
        private void OnceCompleteGame()
        {
            _gameStats.IsCompletedGame.Value = true;
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
            _gameStats.CountCurrentBonuses.Value++;
            _gameStats.SaveValues();
        }
    }
}