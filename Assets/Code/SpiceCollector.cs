using System;
using UnityEngine;

namespace Code
{
    public class SpiceCollector : MonoBehaviour
    {
        public float range;
        
        [NonSerialized]
        public bool isEnabled = false;

        public float cooldownTotal;
        private float cooldownCurrent;

        [NonSerialized]
        public float total = 0;

        public void Update()
        {
            cooldownCurrent += Time.deltaTime;

            if (isEnabled && cooldownCurrent > cooldownTotal)
            {
                // get nearby spice and collect it
                var spice = TerrainUtils.GetNearestSpice(transform.position, range);

                if (spice != null)
                {
                    total += 1;
                    spice.Collect();
                    cooldownCurrent = 0;
                }
                
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}