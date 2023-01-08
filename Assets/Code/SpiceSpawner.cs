using MyBox;
using UnityEngine;

namespace Code
{
    public interface ISpiceSource
    {
        void Collect(Spice spice);
    }
    
    public class SpiceSpawner : MonoBehaviour, ISpiceSource
    {
        public bool spawnsOnStart;
        public bool spawnsOverTime;

        public float cooldownTotal;
        private float cooldownCurrent;

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
            Spawn(Random.Range(minSpawn, maxSpawn));
        }
        
        public void Spawn(int spiceSpawnCount)
        {
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

                var spice = spiceInstance.GetComponent<Spice>();
                spice.source = this;
            }
        }

        public void Start()
        {
            if (spawnsOnStart)
            {
                Spawn();
            }
        }

        public void Collect(Spice spice)
        {
            GameObject.Destroy(spice.gameObject);
        }
    }
}