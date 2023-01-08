using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WormController : MonoBehaviour
{
    public CharacterController controller;

    public float baseSpeed;
    public float speed;

    public float rotationSpeed;

    // Update is called once per frame
    void Update()
    {
        var totalSpeed = speed + baseSpeed;

        var dt = Time.deltaTime;
        
        if (Keyboard.current.upArrowKey.isPressed)
        {
            controller.Move(transform.forward * totalSpeed * dt);
            // controller.SimpleMove(speed);
        }    
        
        if (Keyboard.current.leftArrowKey.isPressed)
        {
            // controller.Move(transform.right * -1f * speed * Time.deltaTime);
            // controller.SimpleMove(speed);
        }    
        
        if (Keyboard.current.rightArrowKey.isPressed)
        {
            transform.localEulerAngles += new Vector3(0, rotationSpeed * dt, 0);

            // var rotation = Quaternion.RotateTowards(transform.rotation, )

            // controller.Move(transform.right * speed * Time.deltaTime);
            // controller.SimpleMove(speed);
        }    
    }
}
