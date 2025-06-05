using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace KnifeHit.Scripts.Levels
{
    public class LevelPlayer : MonoBehaviour
    {
        [SerializeField] private Transform target;
        
        private CancellationTokenSource _cancellation;

        private void Awake()
        {
            /*
            var ed = PlayerPrefs.GetString("Editor");
            if (!string.IsNullOrEmpty(ed))
            {
                PlayLevel(ed);
            }
            */
        }

        public void PlayLevel(string levelData , Action<TargetMoving> OnStep = null)
        {
            _cancellation?.Cancel();
            _cancellation = new CancellationTokenSource();
            target.rotation = Quaternion.identity;
            PlayLevelAsync(levelData , OnStep , _cancellation.Token).Forget();
        }
        
        private async UniTask PlayLevelAsync(string levelData , Action<TargetMoving> onStep , CancellationToken token)
        {
            var parse = LevelParser.ParseInput(levelData);
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
        
        public async UniTask PlayStep(TargetMoving stepData , CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return;
            
            await RotateObjectAsync(stepData, token);
        }

        private void OnDestroy()
        {
            _cancellation?.Cancel();
        }

        public async UniTask RotateObjectAsync(TargetMoving targetAngle, CancellationToken token)
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
            
            while (elapsedTime < targetAngle.Duretion)
            {
                if(token.IsCancellationRequested)
                    return;
                
                var t = elapsedTime / targetAngle.Duretion;
                var z = current + (targetRot - current) * t ;
                
                target.rotation = Quaternion.Euler(0,0,z);
                
                elapsedTime += Time.deltaTime;
                await UniTask.Yield();
            }
            
            target.rotation = Quaternion.Euler(0,0,endRotation);
        }
    }
}