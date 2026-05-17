using TriInspector;
using UnityEngine;

namespace KnifeHit.Scripts.Game
{
    public class CompleteLevelAnimation : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D body;
        [SerializeField] private Collider2D colliderNew;
        [SerializeField] private TargetObject knife;

        [SerializeField] private Vector2 minMaxX;
        [SerializeField] private Vector2 minMaxY;
        
        [SerializeField] private Vector2 minMaxVelocity;
        
        [Button]
        public void StartAnimation()
        {
            knife.IsComplete = true;
            
            body.bodyType = RigidbodyType2D.Dynamic;
            colliderNew.enabled = false;
            body.linearVelocity = new Vector2(Random.Range(minMaxX.x, minMaxX.y), Random.Range(minMaxY.x, minMaxY.y));
            body.gravityScale = 1;
            body.angularVelocity = Random.Range(minMaxVelocity.x, minMaxVelocity.y);
        }

        public void StartAnimationHitToOtherKnife()
        {
            if(colliderNew == null)
                return;
            
            knife.IsComplete = true;
            
            body.bodyType = RigidbodyType2D.Dynamic;
            colliderNew.enabled = false;
            body.linearVelocity = new Vector2(Random.Range(0 , 100) > 50 ? 3 : -3 , 5);
            body.gravityScale = 1;
            body.angularVelocity = Random.Range(0 , 100) > 50 ? 300 : -300;
        }
    }
}