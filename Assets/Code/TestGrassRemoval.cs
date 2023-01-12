using System;
using MyBox;
using UnityEngine;

namespace Code
{
    public class TestGrassRemoval : MonoBehaviour
    {
        public float density = 0.25f;

        public int value = 1;

        private int[,] detailLayer;

        public Vector2Int test;
        public Vector2Int size;
        // public Vector2 resolution = new Vector2(1, 1);

        public float minHeight, maxHeight;

        public bool regenOnStart = false;
        
        private void Start()
        {
            // Generate grass
            if (regenOnStart)
                RegenerateGrass();
        }

        [ButtonMethod()]
        public void RegenerateGrass()
        {
            var terrain = Terrain.activeTerrain;
            var terrainData = terrain.terrainData;
            
            detailLayer = terrainData.GetDetailLayer(0, 0, terrainData.detailWidth, terrainData.detailHeight, 0);

            var resolution = new Vector3
            {
                x = terrainData.size.x / terrainData.detailResolution,
                y = terrainData.size.y / terrainData.detailResolution,
                z = terrainData.size.z / terrainData.detailResolution,
            };

            for (int i = 0; i < detailLayer.GetLength(0); i++)
            {
                for (int j = 0; j < detailLayer.GetLength(1); j++)
                {
                    detailLayer[i, j] = 0;

                    var height = Terrain.activeTerrain.SampleHeight(new Vector3(j * resolution.x, 0, i * resolution.z));

                    if (height >= minHeight && height <= maxHeight)
                    {
                        if (UnityEngine.Random.Range(0f, 1f) < density)
                        {
                            detailLayer[i, j] = value;
                        }
                    }
                }
            }

            terrainData.SetDetailLayer(0, 0, 0, detailLayer);
        }

        private void Update()
        {
            var terrain = Terrain.activeTerrain;
            var terrainData = terrain.terrainData;
            
            var resolution = new Vector3
            {
                x = terrainData.size.x / terrainData.detailResolution,
                y = 0,
                z = terrainData.size.z / terrainData.detailResolution,
            };
            
            test = new Vector2Int(Mathf.RoundToInt(transform.position.x / resolution.x), 
                Mathf.RoundToInt(transform.position.z / resolution.z));
            
            var detailLayer = terrainData.GetDetailLayer(test.x, test.y, Mathf.RoundToInt(size.x / resolution.x), 
                Mathf.RoundToInt(size.y / resolution.z), 0);
            
            for (int i = 0; i < detailLayer.GetLength(0); i++)
            {
                for (int j = 0; j < detailLayer.GetLength(1); j++)
                {
                    detailLayer[i, j] = 0;
                }
            }
            
            terrainData.SetDetailLayer(test.x, test.y, 0, detailLayer);
        }

        private void OnDestroy()
        {
            var terrain = Terrain.activeTerrain;

            if (terrain == null)
            {
                return;
            }
            
            var data = terrain.terrainData;
            var detailLayer = data.GetDetailLayer(0, 0, data.detailWidth, data.detailHeight, 0);

            for (var i = 0; i < detailLayer.GetLength(0); i++)
            {
                for (var j = 0; j < detailLayer.GetLength(1); j++)
                {
                    detailLayer[i, j] = 0;
                }
            }

            data.SetDetailLayer(0, 0, 0, detailLayer);
        }
    }
}