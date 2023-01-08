using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public Transform target;

    public Transform cameraTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cameraTransform.position = target.position + new Vector3(0, 100, 0);
    }
}
