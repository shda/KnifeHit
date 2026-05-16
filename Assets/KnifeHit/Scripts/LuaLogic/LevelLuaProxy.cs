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
        [Inject] private RotatorHandler rotatorHandler;
        [Inject] private ListBonuses listBonuses;
        [Inject] private KnifeSpawner listKnifes;
        [Inject] private Target target;
        [Inject] private GameStats gameStats;
        
        [LuaMember]
        private void SetCountUserKnives(int count, CancellationToken cancellation)
        {
            if (cancellation.IsCancellationRequested)
                return;
            
            gameStats.CountAllUserKnives.SetValueAndForceNotify(count);
            gameStats.CountUserKnives.SetValueAndForceNotify(count);
        }
        
        
        [LuaMember]
        public void SetKnifeSpeed(float speed)
        {
            listKnifes.KnifeSpeed = speed;
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
            var bonus = Object.Instantiate(listBonuses.GetWithOverflow(index));
            //Todo fixed
            bonus.OnReturnToPool = o =>
            {
                Object.DestroyImmediate(o.gameObject);
            };
            target.AddObject(bonus, angle);
        }

        [LuaMember]
        private void SetTargetSkin(int skinIndex, CancellationToken cancellation)
        {
            if (cancellation.IsCancellationRequested)
                return;
            
            target.SetSkin(skinIndex);
            Debug.Log("SetTargetSkin " + skinIndex);
        }

        [LuaMember]
        private void SetUserKnifeSkin(int skinIndex , CancellationToken cancellation)
        {
            if (cancellation.IsCancellationRequested)
                return;
            
            Debug.Log("SetUserKnifeSkin");
            listKnifes.SkinIndex = skinIndex;
           // game.SetKnifeSkin(skinIndex);
        }
   
        [LuaMember]
        private void SetObstacle(int index, int angle , CancellationToken cancellation)
        {
            if (cancellation.IsCancellationRequested)
                return;

            Debug.Log("SetObstacle");

            var knife = listKnifes.GetKnife(); // Object.Instantiate(listKnifes.GetWithOverflow(index));
            knife.SetSkinIndex(index);
            knife.IsMoving = false;
            knife.SetStaticRigidbody2D();

            target.AddObject(knife, angle, 180);
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

            await rotatorHandler.PlayStepAsync(result, cancellation);
    
            return 0;
        }
       
    }
}