using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WormController : MonoBehaviour
{
    public PlayerInput playerInput;
    public CharacterController controller;

    public float baseSpeed;
    public float maxSpeed;

    private float speed;

    public float rotationSpeed;

    // Update is called once per frame
    void Update()
    {
        var totalSpeed = baseSpeed + speed;

        var dt = Time.deltaTime;

        var accelerate = playerInput.actions["Accelerate"];
        var turnRight = playerInput.actions["TurnRight"];
        var turnLeft = playerInput.actions["TurnLeft"];

        var rotation = 0.0f;
        
        if (accelerate.IsPressed())
        {
            controller.Move(transform.forward * totalSpeed * dt);
            // controller.SimpleMove(speed);
        }    
        
        if (turnLeft.IsPressed())
        {
            rotation += 1f;
        }    
        
        if (turnRight.IsPressed())
        {
            rotation += -1f;
        }    
        
        transform.localEulerAngles += new Vector3(0, rotationSpeed * rotation * dt, 0);
        controller.Move(transform.forward * totalSpeed * dt);
    }
}
