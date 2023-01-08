using System;
using UnityEngine;

namespace Code
{
    public class Spice : MonoBehaviour
    {
        [NonSerialized]
        public ISpiceSource source;
        
        public void Collect()
        {
            source?.Collect(this);
        }
    }
}