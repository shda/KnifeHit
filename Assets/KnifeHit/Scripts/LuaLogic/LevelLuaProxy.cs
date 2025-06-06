using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using KnifeHit.Scripts.Levels;
using KnifeHit.Scripts.Lists;
using Lua;
using UnityEngine;
using Object = UnityEngine.Object;

namespace KnifeHit.Scripts.LuaLogic
{
    [Serializable]
    [LuaObject]
    public partial class LevelLuaProxy
    {
        [SerializeField] private RotatorHandler rotatorHandler;
        [SerializeField] private ListBonuses listBonuses;
        [SerializeField] private ListKnifes listKnifes;
        [SerializeField] private Target target;
        [SerializeField] private Game game;
        
        [LuaMember]
        private void SetCountUserKnives(int count, CancellationToken cancellation)
        {
            if (cancellation.IsCancellationRequested)
                return;

            game.SetCountAllKnives(count , count);
        }
        
        
        [LuaMember]
        public void SetKnifeSpeed(float speed)
        {
            game.SetKnifeSpeed(speed);
        }

        [LuaMember]
        public void SetDelayBetweenKnives(float delayBetweenKnifes)
        {
            game.SetDelayBetweenKnives(delayBetweenKnifes);
        }
        
        [LuaMember]
        private void SetBonus(int index, int angle , CancellationToken cancellation)
        {
            if (cancellation.IsCancellationRequested)
                return;

            Debug.Log("SetBonus");
            var bonus = Object.Instantiate(listBonuses.GetWithOverflow(index));
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
            game.SetKnifeSkin(skinIndex);
        }
   
        [LuaMember]
        private void SetObstacle(int index, int angle , CancellationToken cancellation)
        {
            if (cancellation.IsCancellationRequested)
                return;

            Debug.Log("SetObstacle");
            
            var knife = Object.Instantiate(listKnifes.GetWithOverflow(index));
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