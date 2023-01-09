using System;
using UnityEngine;

namespace Code
{
    public class ShaiHuludTeeth : MonoBehaviour
    {
        public float rotationSpeed;

        private void Update()
        {
            var rotation = transform.localEulerAngles;
            rotation.y += rotationSpeed * Time.deltaTime;
            transform.localEulerAngles = rotation;
        }
    }
}