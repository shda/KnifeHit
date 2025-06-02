using System;
using UnityEngine;

namespace KnifeHit.Scripts
{
    public class RotatorLastFixUpdate : MonoBehaviour
    {
        [SerializeField] private float speed = 100f;

        private void FixedUpdate()
        {
            transform.Rotate(0f, 0f, speed * Time.deltaTime);
        }
    }
}