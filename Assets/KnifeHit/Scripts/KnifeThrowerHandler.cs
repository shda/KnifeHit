using System;
using UnityEngine;
using Zenject;

namespace KnifeHit.Scripts
{
    public class KnifeThrowerHandler : IInitializable
    {
        private readonly KnifeSpawner _knifeSpawner;
        private readonly InputHandler _inputHandler;
        public KnifeThrowerHandler(KnifeSpawner knifeSpawner , InputHandler inputHandler)
        {
            _knifeSpawner = knifeSpawner;
            _inputHandler = inputHandler;
        }
        
        public void Initialize()
        {
            _knifeSpawner.OnKnifeCollisionToOther = KnifeCollisionToOther;
            _knifeSpawner.OnKnifeTriggerToOther = KnifeTriggerToOther;
        }
        
        private void KnifeTriggerToOther(Knife arg1, Collider2D arg2)
        {
            throw new NotImplementedException();
        }
        private void KnifeCollisionToOther(Knife arg1, Collision2D arg2)
        {
            throw new NotImplementedException();
        }
        
        public void SetKnifeSpeed(float speed)
        {
            _knifeSpawner.KnifeSpeed = speed;
        }
        
        public void SetKnifeSkin(int skinIndex)
        {
            _knifeSpawner.SkinIndex = skinIndex;
        }

        public void ThrowKnife()
        {
            
        }
    }
}