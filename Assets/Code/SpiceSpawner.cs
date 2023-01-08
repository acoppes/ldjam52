using MyBox;
using UnityEngine;

namespace Code
{
    public class SpiceSpawner : MonoBehaviour
    {
        public bool spawnsOnStart;

        public GameObject spicePrefab;

        public float spawnRadius;

        public int minSpawn;
        public int maxSpawn;
        
        // list<spice>

        [ButtonMethod]
        public void DestroySpice()
        {
            while (transform.childCount > 0)
            {
                var child = transform.GetChild(0);
                DestroyImmediate(child.gameObject);
            }
        }
        
        [ButtonMethod]
        public void Spawn()
        {
            var spiceSpawnCount = Random.Range(minSpawn, maxSpawn);
            var spawnerPosition = new Vector2(transform.position.x, transform.position.z);
            
            for (var i = 0; i < spiceSpawnCount; i++)
            {
                var position = spawnerPosition + Random.insideUnitCircle * spawnRadius;
                    
                var spiceInstance = Instantiate(spicePrefab, transform);

                var height = 0.0f;
                
                if (Terrain.activeTerrain != null)
                {
                    height = Terrain.activeTerrain.SampleHeight(new Vector3(position.x, 0,
                        position.y));
                }
                var position3d = new Vector3(position.x, height, position.y);

                spiceInstance.transform.position = position3d + new Vector3(0, 0.1f, 0);
                spiceInstance.transform.localEulerAngles = new Vector3(90, Random.Range(0, 360), 0);
            }
        }

        public void Start()
        {
            if (spawnsOnStart)
            {
                Spawn();
            }
        }
    }
}