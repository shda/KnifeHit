using System;
using KnifeHit.Scripts.Collections;
using UnityEngine;

namespace KnifeHit.Scripts.Game
{
    public class Knife : TargetObject
    {
        [SerializeField] private ListSkins listKnifeSkins;
        [SerializeField] private SpriteRenderer knifeSpriteRenderer;
        [SerializeField] private Vector2 movingSpeed;
        
        public Action<Knife , Collision2D> OnCollision;
        public Action<Knife , Collider2D> OnTriggerEnter;
        
        public bool IsMoving { get; set; }

        private void OnEnable()
        {
            ResetToDefault();
        }

        public void SetSkinIndex(int index)
        {
            knifeSpriteRenderer.sprite = listKnifeSkins.GetWithOverflow(index);
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
            OnCollision?.Invoke(this,other);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
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

        public void ResetToDefault()
        {
            IsComplete = false;
            
            transform.SetParent(null);
            OnTriggerEnter = null;
            OnCollision = null;
            
            body.gravityScale = 0;
            body.bodyType = RigidbodyType2D.Dynamic;
            SwitchCollider(true);
        }
    }
}