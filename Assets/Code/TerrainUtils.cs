using UnityEngine;
using Vertx.Debugging;

namespace Code
{
    public static class TerrainUtils
    {
        private const float ProjectionStartingDistance = 100f;
        
        private static readonly RaycastHit[] RaycastHits = new RaycastHit[10];
        private static readonly Collider[] Colliders = new Collider[100];
        
        private static readonly LayerMask TerrainLayerMask = LayerMask.GetMask("Terrain");
        private static readonly LayerMask SpiceLayerMask = LayerMask.GetMask("Spice");

        public static Vector3 RandomPositionInsideTerrain()
        {
            var bounds = new Bounds(Vector3.zero, Vector3.one);
            
            if (Terrain.activeTerrain != null)
            {
                bounds = Terrain.activeTerrain.terrainData.bounds;
            }
            else
            {
                var terrainObject = GameObject.Find("Terrain");
                if (terrainObject != null)
                {
                    bounds = terrainObject.GetComponent<MeshCollider>().bounds;
                }
            }
            
            return new Vector3(UnityEngine.Random.Range(bounds.min.x, bounds.max.x), 0,
                UnityEngine.Random.Range(bounds.min.z, bounds.max.z));
        }

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

        public static Spice GetNearestSpice(Vector3 position, float range)
        {
            var collidersFound = DrawPhysics.OverlapSphereNonAlloc(position, range, Colliders, SpiceLayerMask,
                QueryTriggerInteraction.Collide);

            if (collidersFound == 0)
                return null;

            var collider = Colliders[Random.Range(0, collidersFound)];
            return collider.GetComponent<Spice>();

            // for (int i = 0; i < collidersFound; i++)
            // {
            //     var collider = Colliders[i];
            //     var spice = collider.GetComponent<Spice>();
            //     if (spice == null)
            //     {
            //         continue;
            //     }
            //     
            //     // TODO: nearest
            //
            //     return spice;
            // }
            //
            // return null;
        }
    }
}