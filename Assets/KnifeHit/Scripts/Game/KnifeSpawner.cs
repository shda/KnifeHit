using System;
using Common.Scripts;
using UnityEngine;

namespace KnifeHit.Scripts.Game
{
    public class KnifeSpawner : MonoBehaviour
    {
        [SerializeField] private Transform startSpawnKnife;
        [SerializeField] private PoolGameObjectsComponent<Knife> knifes;

        private int _skinIndex;
        private float _knifeSpeed;

        public int SkinIndex
        {
            get => _skinIndex;
            set
            {
                _skinIndex = value;
                UpdateCurrentKnife();
            }
        }

        public float KnifeSpeed
        {
            get => _knifeSpeed;
            set
            {
                _knifeSpeed = value;
                UpdateCurrentKnife();
            }
        }

        private Knife _currentKnife;

        public Action<Knife, Collision2D> OnKnifeCollisionToOther;
        public Action<Knife, Collider2D> OnKnifeTriggerToOther;

        private void Awake()
        {
            knifes.Init();
        }

        public void PrepareNewKnife()
        {
            _currentKnife = GetKnife();
            UpdateCurrentKnife();
        }

        public void UpdateCurrentKnife()
        {
            if(!_currentKnife)
                PrepareNewKnife();
            _currentKnife.SetSkinIndex(SkinIndex);
            _currentKnife.SwitchCollider(false);
            _currentKnife.SetVelocity(KnifeSpeed);
            _currentKnife.transform.rotation = Quaternion.identity;
            _currentKnife.transform.position = startSpawnKnife.position;
            _currentKnife.OnCollision = (knife, coll) => OnKnifeCollisionToOther.Invoke(knife, coll);
            _currentKnife.OnTriggerEnter = (knife, coll) => OnKnifeTriggerToOther?.Invoke(knife, coll);
        }

        public Knife GetKnife()
        {
            var knife = knifes.Get();
            knife.OnReturnToPool = OnReturnToPool;
            knife.ResetToDefault();
            
            return knife;
        }
        
        private void OnReturnToPool(TargetObject obj)
        {
            knifes.Release(obj as Knife);
        }

        public Knife ThrowKnife()
        {
            if (!_currentKnife) 
                return null;

            var knife = _currentKnife;
            _currentKnife = null;
            
            knife.SwitchCollider(true);
            knife.KnifeThrow();
            return knife;
        }
        
        public void CreateNewKnife()
        {
            if (!_currentKnife)
            {
                PrepareNewKnife();
            }
        }
    }
}