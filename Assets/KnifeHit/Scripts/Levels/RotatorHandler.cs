using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace KnifeHit.Scripts.Levels
{
    public class RotatorHandler : MonoBehaviour
    {
        [SerializeField] private Transform target;
        
        private CancellationTokenSource _cancellation;
        
        public void PlayLevel(string levelData , Action<RotationData> OnStep = null)
        {
            _cancellation?.Cancel();
            _cancellation = new CancellationTokenSource();
            target.rotation = Quaternion.identity;
            PlayLevelAsync(levelData , OnStep , _cancellation.Token).Forget();
        }
        
        private async UniTask PlayLevelAsync(string levelData , Action<RotationData> onStep , CancellationToken token)
        {
            var parse = RotationsParser.ParseInput(levelData);
            var indexStep = 0;
            while (!token.IsCancellationRequested)
            {
                var step = parse[indexStep % parse.Count];
                onStep?.Invoke(step);
                
                await PlayStep(step , token);
                indexStep++;
                
                await UniTask.Yield();
            }
        }
        
        public async UniTask PlayStep(RotationData stepData , CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return;
            
            await RotateObjectAsync(stepData, token);
        }

        private void OnDestroy()
        {
            _cancellation?.Cancel();
        }

        public async UniTask RotateObjectAsync(RotationData targetAngle, CancellationToken token)
        {
            if(token.IsCancellationRequested)
                return;
            
            var startRotation = target.eulerAngles;
            var endRotation = targetAngle.Angle;
            
            if (targetAngle.IsOffset)
            {
                endRotation = startRotation.z + targetAngle.Angle;
            }
            
            float elapsedTime = 0f;
            
            var current = target.eulerAngles.z;
            var targetRot = endRotation;
            
            while (elapsedTime < targetAngle.Duration)
            {
                if(token.IsCancellationRequested)
                    return;
                
                var t = elapsedTime / targetAngle.Duration;
                var z = current + (targetRot - current) * t ;
                
                target.rotation = Quaternion.Euler(0,0,z);
                
                elapsedTime += Time.deltaTime;
                await UniTask.Yield();
            }
            
            target.rotation = Quaternion.Euler(0,0,endRotation);
        }
    }
}