using UnityEngine;

namespace KnifeHit.Scripts
{
    public class TargetObject : MonoBehaviour
    {
        [SerializeField] protected Rigidbody2D body;
        [SerializeField] protected Collider2D colliderNew;
        [SerializeField] protected CompleteLevelAnimation completeLevelAnimation;
        
        public bool IsComplete { get; set; }
        public void PlayCompleteAnimation()
        {
            completeLevelAnimation.StartAnimationHitToOtherKnife();
            gameObject.transform.SetParent(null);
        }
    }
}