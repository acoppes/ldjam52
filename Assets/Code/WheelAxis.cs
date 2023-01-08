using System;
using MyBox;
using UnityEngine;
using Vertx.Debugging;

namespace Code
{
    public class WheelAxis : MonoBehaviour
    {
        public Wheel[] wheels;

        [ReadOnly]
        public Vector3 up;

        public Transform axisCenter;

        private void Update()
        {
            var middlePoint = Vector3.zero;
            var backMiddlePoint = Vector3.zero;

            foreach (var wheel in wheels)
            {
                middlePoint += wheel.transform.position;
            }
            
            // foreach (var wheel in backWheels)
            // {
            //     backMiddlePoint += wheel.transform.position;
            // }

            middlePoint /= wheels.Length;
            // backMiddlePoint /= backWheels.Length;

            // var middlePoint = (frontMiddlePoint + backMiddlePoint) * 0.5f;
           // Debug.DrawLine(transform.position, middlePoint);
            
            up = axisCenter.position - middlePoint;
            
            D.raw(new Shape.Sphere(axisCenter.position, 0.1f), Color.red);
            D.raw(new Shape.Sphere(middlePoint, 0.1f), Color.red);
            D.raw(new Shape.Line(axisCenter.position, axisCenter.position + up * 5f), Color.red);
            
            // Debug.DrawLine(transform.position, transform.position + up * 5f);
        }
    }
}