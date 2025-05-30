using System;
using UnityEngine;

namespace KnifeHit.Scripts
{
    public class Knife : MonoBehaviour
    {
        [SerializeField] private Vector2 movingSpeed;
        [SerializeField] private Rigidbody2D body;

        public Action<Collision2D> OnCollision;
        public Action<Collider2D> OnTriggerEnter;

        public bool IsMoving { get; set; }

        public void KnifeThrow()
        {
            IsMoving = true;
        }

        private void FixedUpdate()
        {
            if (body.bodyType == RigidbodyType2D.Static)
                return;
            
            body.linearVelocity = IsMoving ? movingSpeed : Vector2.zero;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            Debug.Log(other.collider.gameObject.name);
            OnCollision?.Invoke(other);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log(other.gameObject.name);
            OnTriggerEnter?.Invoke(other);
        }

        public void SetStaticRigidbody2D()
        {
            body.bodyType = RigidbodyType2D.Static;
        }
    }
}