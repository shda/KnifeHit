using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace KnifeHit.Scripts.Levels
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField] private Transform target;
        private CancellationTokenSource _cancellation;

        public async UniTask PlayStepAsync(RotationData stepData , CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return;
            
            await RotationAsync(stepData, token);
        }

        private async UniTask RotationAsync(RotationData targetAngle, CancellationToken token)
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
        
        private void OnDestroy()
        {
            _cancellation?.Cancel();
        }
    }
}