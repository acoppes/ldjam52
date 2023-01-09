using System;
using MyBox;
using UnityEngine;
using Vertx.Debugging;

namespace Code
{
    public class WheelAxis : MonoBehaviour
    {
        public Transform a, b;

        [ReadOnly]
        public Vector3 up;

        private void Update()
        {
            D.raw(new Shape.Sphere(transform.position, 0.1f), Color.white);
            
            // var middlePoint = Vector3.zero;
            // var backMiddlePoint = Vector3.zero;

            var difference = a.position - b.position;
            var direction = difference.normalized;

            var rotation = Quaternion.AngleAxis(90, transform.forward);
            var rotatedDirection = rotation * direction;
            
            D.raw(new Shape.Line(a.position, b.position), Color.black);
            D.raw(new Shape.Line(transform.position, transform.position + rotatedDirection * 2f), Color.red);

            up = rotatedDirection;

            transform.position = (a.position + b.position) * 0.5f;

            //  foreach (var axisObject in axisObjects)
            //  {
            //      middlePoint += axisObject.position;
            //  }
            //  
            //  // foreach (var wheel in backWheels)
            //  // {
            //  //     backMiddlePoint += wheel.transform.position;
            //  // }
            //
            //  middlePoint /= axisObjects.Length;
            //  // backMiddlePoint /= backWheels.Length;
            //
            //  // var middlePoint = (frontMiddlePoint + backMiddlePoint) * 0.5f;
            // // Debug.DrawLine(transform.position, middlePoint);
            //  
            //  up = middlePoint - axisCenter.position;
            //  
            //  
            //  D.raw(new Shape.Sphere(middlePoint, 0.1f), Color.yellow);
            //  D.raw(new Shape.Line(axisCenter.position, axisCenter.position + up * 5f), Color.red);
            //  
            //  // Debug.DrawLine(transform.position, transform.position + up * 5f);
        }
    }
}