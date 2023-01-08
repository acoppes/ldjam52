using System.Collections;
using System.Collections.Generic;
using MyBox;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class WormController : MonoBehaviour
{
    public PlayerInput playerInput;
    public CharacterController controller;

    public float baseSpeed;
    public float maxSpeed;
    public float minSpeed;

    public float acceleration;
    public float deceleration;
    
    [MyBox.ReadOnly]
    public float speed;

    [MyBox.ReadOnly]
    public float rotationSpeed;

    public float baseRotationSpeed;

    public float gravity;

    // Update is called once per frame
    void Update()
    {
        var totalSpeed = baseSpeed + speed;

        var dt = Time.deltaTime;

        var accelerate = playerInput.actions["Accelerate"];
        var reverse = playerInput.actions["Reverse"];

        var turnRight = playerInput.actions["TurnRight"];
        var turnLeft = playerInput.actions["TurnLeft"];

        var rotationDirection = 0.0f;
        
        if (accelerate.IsPressed())
        {
            speed += acceleration * dt;
        }
        else if (reverse.IsPressed())
        {
            speed -= acceleration * dt;
        }
        else
        {
            if (speed > 0)
            {
                speed -= deceleration * dt;
            }
            else
            {
                speed += deceleration * dt;
            }
        }

        speed = Mathf.Clamp(speed, minSpeed, maxSpeed);
        
        if (turnLeft.IsPressed())
        {
            rotationDirection += 1f;
        }    
        
        if (turnRight.IsPressed())
        {
            rotationDirection += -1f;
        }

        rotationSpeed = baseRotationSpeed + speed * 2;
        
        transform.localEulerAngles += new Vector3(0, rotationSpeed * rotationDirection * dt, 0);

        var motion = transform.forward * totalSpeed * dt;
        motion += new Vector3(0, gravity * dt, 0);

        controller.Move(motion);
    }
}
