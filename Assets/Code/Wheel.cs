using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code
{
    public class Wheel : MonoBehaviour
    {
        public float projectionStartingDistance = 10f;

        private LayerMask terrainLayerMask;
        private RaycastHit[] raycastHits = new RaycastHit[1];

        public LineRenderer trace;

        public bool traceDisabled;

        public float traceSpawnDistance = 1f;
        
        private Vector3 lastTraceSpawnPosition;
        
        private List<Vector3> tracePositions = new List<Vector3>();

        private void Awake()
        {
            terrainLayerMask = LayerMask.GetMask("Terrain");

            lastTraceSpawnPosition = transform.position;
            lastTraceSpawnPosition.y = 0;

            // tracePositions = new NativeArray<Vector3>(200, Allocator.Persistent);
        }

        // private void OnDestroy()
        // {
        //     tracePositions.Dispose();
        // }

        private void Update()
        {
            Ray ray = new Ray()
            {
                origin = transform.position + Vector3.up * projectionStartingDistance,
                direction = Vector3.up * -1f
            };

            var hits = Physics.RaycastNonAlloc(ray, raycastHits, projectionStartingDistance* 2, 
                terrainLayerMask);
            if (hits > 0)
            {
                transform.position = raycastHits[0].point;
            }

            var traceSpawnPosition = transform.position;
            traceSpawnPosition.y = 0;

            if (!traceDisabled && Vector3.Distance(lastTraceSpawnPosition, traceSpawnPosition) > traceSpawnDistance)
            {
                tracePositions.Add(transform.position + new Vector3(0, 0.1f, 0));
                trace.positionCount = tracePositions.Count;
                trace.SetPositions(tracePositions.ToArray());
                // trace.Simplify(0.1f);
                
                lastTraceSpawnPosition = traceSpawnPosition;
            }
        }
    }
}