using System;
using UnityEngine;

namespace KnifeHit.Scripts
{
    public class Knife : TargetObject
    {
        [SerializeField] private Vector2 movingSpeed;
        
        public Action<Knife , Collision2D> OnCollision;
        public Action<Knife , Collider2D> OnTriggerEnter;
        
        public bool IsMoving { get; set; }

        private void OnEnable()
        {
            body.gravityScale = 0;
            IsComplete = false;
        }

        public void KnifeThrow()
        {
            IsMoving = true;
        }
        
        public void SwitchCollider(bool isEnable)
        {
            colliderNew.enabled = isEnable;
        }

        private void FixedUpdate()
        {
            if (body.bodyType == RigidbodyType2D.Static)
                return;

            if (!IsComplete)
            {
                body.linearVelocity = IsMoving ? movingSpeed : Vector2.zero;
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
           // Debug.Log(other.collider.gameObject.name);
            OnCollision?.Invoke(this,other);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
           // Debug.Log(other.gameObject.name);
            OnTriggerEnter?.Invoke(this, other);
        }

        public void SetStaticRigidbody2D()
        {
            body.bodyType = RigidbodyType2D.Static;
        }

        public void SetVelocity(float knifeSpeed)
        {
            movingSpeed = new Vector2(0, knifeSpeed);
        }
    }
}