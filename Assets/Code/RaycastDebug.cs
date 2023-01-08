using System;
using UnityEngine;
using Vertx.Debugging;

namespace Code
{
    [ExecuteAlways]
    public class RaycastDebug : MonoBehaviour
    {
        private void Update()
        {
            var hit = transform.position;
            
            var height = Terrain.activeTerrain.SampleHeight(hit);
            hit.y = height;
            
            D.raw(new Shape.Sphere(hit), Color.blue);
            // D.raw(new Shape.Line(hit, hit + hit.normal), Color.blue);
        }
    }
}