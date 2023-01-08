using System;
using Gemserk.Gameplay;
using UnityEngine;

namespace Code
{
    public class SpiceCollector : MonoBehaviour
    {
        public float startingSpice;
        
        public float range;

        [NonSerialized]
        public bool isEnabled = false;

        public Cooldown harvestCooldown = new Cooldown(0.5f);

        [NonSerialized]
        public float total = 0;

        [NonSerialized]
        public bool isHarvesting;

        public Cooldown stopHarvestingCooldown = new Cooldown(0.3f);

        private void Start()
        {
            total = startingSpice;
        }

        public void Update()
        {
            stopHarvestingCooldown.Increase(Time.deltaTime);
            harvestCooldown.Increase(Time.deltaTime);
            
            if (isEnabled && harvestCooldown.IsReady)
            {
                // get nearby spice and collect it
                var spice = TerrainUtils.GetNearestSpice(transform.position, range);

                if (spice != null)
                {
                    total += 1;
                    spice.Collect();
                    
                    isHarvesting = true;
                    
                    harvestCooldown.Reset();
                    stopHarvestingCooldown.Reset();
                }
            }

            if (isHarvesting && stopHarvestingCooldown.IsReady)
            {
                isHarvesting = false;
                stopHarvestingCooldown.Reset();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}