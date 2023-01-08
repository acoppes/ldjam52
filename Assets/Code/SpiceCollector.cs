using System;
using UnityEngine;

namespace Code
{
    public class SpiceCollector : MonoBehaviour
    {
        public float startingSpice;
        
        public float range;

        [NonSerialized]
        public bool isEnabled = false;

        public float cooldownTotal;
        private float cooldownCurrent;

        [NonSerialized]
        public float total = 0;

        [NonSerialized]
        public bool isHarvesting;
        
        public float stopHarvestingDelay;
        private float stopHarvestingTime;

        private void Start()
        {
            total = startingSpice;
        }

        public void Update()
        {
            cooldownCurrent += Time.deltaTime;
            stopHarvestingTime += Time.deltaTime;
            
            if (isEnabled && cooldownCurrent > cooldownTotal)
            {
                // get nearby spice and collect it
                var spice = TerrainUtils.GetNearestSpice(transform.position, range);

                if (spice != null)
                {
                    total += 1;
                    spice.Collect();
                    cooldownCurrent = 0;

                    isHarvesting = true;

                    stopHarvestingTime = 0;
                }
            }

            if (isHarvesting && stopHarvestingTime > stopHarvestingDelay)
            {
                isHarvesting = false;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}