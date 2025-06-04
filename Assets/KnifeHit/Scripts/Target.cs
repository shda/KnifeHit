using System;
using UnityEngine;

namespace KnifeHit.Scripts
{
    public class Target : MonoBehaviour
    {
        [SerializeField] private float offset;

        private GameObject _obj;
        private int _angle;
        
        public void AddObject(GameObject obj, int angle , float addRotation = 0)
        {
            obj.transform.SetParent(transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            
            //Set position
            obj.transform.rotation = Quaternion.Euler(0,0,-angle);
            obj.transform.position += -obj.transform.up * offset;
            
            //Return rotation
            obj.transform.rotation = Quaternion.Euler(0,0,  obj.transform.eulerAngles.z + 180 + addRotation);
            
            _obj = obj;
            _angle = angle;
        }

        /*
        private void Update()
        {
            if (_obj != null)
            {
                AddObject(_obj , _angle);
            }
        }
        */
    }
}