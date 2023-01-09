using System;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

namespace Code
{
    public class Wheel : MonoBehaviour
    {
        public LineRenderer trace;

        public bool projectToTerrainDisabled;
        public bool traceDisabled;

        public float traceSpawnDistance = 1f;
        
        private Vector3 lastTraceSpawnPosition;
        
        private List<Vector3> tracePositions = new List<Vector3>();

        private void Awake()
        {
            lastTraceSpawnPosition = transform.position;
            lastTraceSpawnPosition.y = 0;
        }

        private void Update()
        {
            if (!projectToTerrainDisabled)
            {
                var height = TerrainUtils.GetHeightAtPosition(transform.position);
                transform.position = transform.position.SetY(height);
            }
            
            // if (TerrainUtils.GetTerrainPoint(transform.position, out var hit))
            // {
            //     transform.position = hit.point;
            // }
            
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