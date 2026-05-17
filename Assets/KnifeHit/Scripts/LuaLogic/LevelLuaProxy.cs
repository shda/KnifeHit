using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using KnifeHit.Scripts.Levels;
using KnifeHit.Scripts.Lists;
using Lua;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace KnifeHit.Scripts.LuaLogic
{
    [Serializable]
    [LuaObject]
    public partial class LevelLuaProxy
    {
        [Inject] private Rotator _rotator;
        [Inject] private ListBonuses _listBonuses;
        [Inject] private KnifeSpawner _listKnifes;
        [Inject] private Target _target;
        [Inject] private GameStats _gameStats;
        
        [LuaMember]
        private void SetCountUserKnives(int count, CancellationToken cancellation)
        {
            if (cancellation.IsCancellationRequested)
                return;
            
            _gameStats.CountAllUserKnives.SetValueAndForceNotify(count);
            _gameStats.CountUserKnives.SetValueAndForceNotify(count);
        }
        
        [LuaMember]
        public void SetKnifeSpeed(float speed)
        {
            _listKnifes.KnifeSpeed = speed;
        }

        [LuaMember]
        public void SetDelayBetweenKnives(float delayBetweenKnifes)
        {
            //game.SetDelayBetweenKnives(delayBetweenKnifes);
        }
        
        [LuaMember]
        private void SetBonus(int index, int angle , CancellationToken cancellation)
        {
            if (cancellation.IsCancellationRequested)
                return;

            Debug.Log("SetBonus");
            var bonus = Object.Instantiate(_listBonuses.GetWithOverflow(index));
            //Todo fixed
            bonus.OnReturnToPool = o =>
            {
                Object.DestroyImmediate(o.gameObject);
            };
            _target.AddObject(bonus, angle);
        }

        [LuaMember]
        private void SetTargetSkin(int skinIndex, CancellationToken cancellation)
        {
            if (cancellation.IsCancellationRequested)
                return;
            
            _target.SetSkin(skinIndex);
            Debug.Log("SetTargetSkin " + skinIndex);
        }

        [LuaMember]
        private void SetUserKnifeSkin(int skinIndex , CancellationToken cancellation)
        {
            if (cancellation.IsCancellationRequested)
                return;
            
            Debug.Log("SetUserKnifeSkin");
            _listKnifes.SkinIndex = skinIndex;
        }
   
        [LuaMember]
        private void SetObstacle(int index, int angle , CancellationToken cancellation)
        {
            if (cancellation.IsCancellationRequested)
                return;

            Debug.Log("SetObstacle");

            var knife = _listKnifes.GetKnife(); 
            knife.SetSkinIndex(index);
            knife.IsMoving = false;
            knife.SetStaticRigidbody2D();

            _target.AddObject(knife, angle, 180);
        }

        
        [LuaMember]
        private async UniTask<int> RotateAsync(string rotationString, CancellationToken cancellation)
        {
            await UniTask.Yield();
            Debug.Log("Rotate");
            
            if (cancellation.IsCancellationRequested)
                return 0;
            
            var result = RotationsParser.ParseLine(rotationString);
            
            if (cancellation.IsCancellationRequested)
                return 0;

            await _rotator.PlayStepAsync(result, cancellation);
            return 0;
        }
       
    }
}