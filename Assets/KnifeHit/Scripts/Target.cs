using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
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
        
        private Vector3 _defaultSize;
        
        private readonly List<TargetObject> _addObjects = new();

        private void Awake()
        {
            _defaultSize =  transform.localScale;
            
            var children = child.Cast<Transform>().ToArray();
            foreach (var c in children)
            {
                DestroyImmediate(c.gameObject);
            }
        }

        public void SetSkin(int index)
        {
            spriteRenderer.sprite = skins.GetWithOverflow(index);
        }

        public void SetDefaultSize()
        {
            transform.localScale = _defaultSize;
        }
        
        public void AddObject(TargetObject obj, int angle , float addRotation = 0)
        {
            obj.transform.SetParent(child);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            
            //Set position
            obj.transform.rotation = Quaternion.Euler(0,0,-angle);
            obj.transform.position += -obj.transform.up * offset;
            
            //Return rotation
            obj.transform.rotation = Quaternion.Euler(0,0,  obj.transform.eulerAngles.z + 180 + addRotation);
            
            _addObjects.Add(obj);
        }
        
        public void RemoveOldObjects()
        {
            foreach (var addObject in _addObjects)
            {
                if (addObject)
                {
                    addObject.ReturnObjectToPool();
                }
            }
            
            _addObjects.Clear();
        }
        
        public void AnimationEndLevelAsync()
        {
            foreach (var obj in _addObjects)
            {
                if (obj)
                {
                    obj.PlayCompleteAnimation();
                }
            }

            transform.DOScale(new Vector3(0, 0, 0), 0.5f);
        }

        public void KnifeHitToTarget(Knife knife)
        {
            knife.IsMoving = false;
            knife.SetStaticRigidbody2D();
            knife.transform.SetParent(child);
            
            hitTargetAnimation.PlayAnimation();
            
           // _addObjects.Add(knife);
        }

        public void AddKnife(Knife knife)
        {
            _addObjects.Add(knife);
        }
    }
}