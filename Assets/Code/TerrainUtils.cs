using UnityEngine;
using Vertx.Debugging;

namespace Code
{
    public static class TerrainUtils
    {
        private const float ProjectionStartingDistance = 100f;
        
        private static readonly RaycastHit[] RaycastHits = new RaycastHit[10];
        
        private static readonly LayerMask TerrainLayerMask = LayerMask.GetMask("Terrain");

        public static float GetHeightAtPosition(Vector3 position)
        {
            if (Terrain.activeTerrain != null)
            {
                return Terrain.activeTerrain.SampleHeight(position);
            }

            if (GetTerrainPoint(position, out var hit))
            {
                return hit.point.y;
            }

            return 0;
        }
        
        public static bool GetTerrainPoint(Vector3 position, out RaycastHit hit)
        {
            hit = new RaycastHit();
            
            var ray = new Ray()
            {
                origin = position + Vector3.up * ProjectionStartingDistance,
                direction = Vector3.up * -1f
            };

            // Terrain.activeTerrain.SampleHeight(position);
            
            var hits = DrawPhysics.RaycastNonAlloc(ray, RaycastHits, ProjectionStartingDistance + 1, 
                TerrainLayerMask);

            if (hits > 0)
            {
                hit = RaycastHits[0];
                return true;
            }

            return false;
        }
    }
}