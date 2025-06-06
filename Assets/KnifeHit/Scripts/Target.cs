using System;
using System.Linq;
using KnifeHit.Scripts.Lists;
using UnityEngine;

namespace KnifeHit.Scripts
{
    public class Target : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Transform child;
        [SerializeField] private ListSkins skins; 
        [SerializeField] private float offset;

        [SerializeField] private HitTargetAnimation hitTargetAnimation;

        public void SetSkin(int index)
        {
            spriteRenderer.sprite = skins.GetWithOverflow(index);
        }
        
        public void AddObject(GameObject obj, int angle , float addRotation = 0)
        {
            obj.transform.SetParent(child);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            
            //Set position
            obj.transform.rotation = Quaternion.Euler(0,0,-angle);
            obj.transform.position += -obj.transform.up * offset;
            
            //Return rotation
            obj.transform.rotation = Quaternion.Euler(0,0,  obj.transform.eulerAngles.z + 180 + addRotation);
        }

        public void RemoveOldObjects()
        {
            var children = child.Cast<Transform>().ToArray();
            foreach (var c in children)
            {
                DestroyImmediate(c.gameObject);
            }
        }

        public void HitToTarget(Knife knife)
        {
            knife.IsMoving = false;
            knife.SetStaticRigidbody2D();
            knife.transform.SetParent(transform);
            
            hitTargetAnimation.PlayAnimation();
        }
    }
}