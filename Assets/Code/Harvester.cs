using UnityEngine;
using UnityEngine.InputSystem;

namespace Code
{
    public class Harvester : MonoBehaviour
    {
        public PlayerInput playerInput;
        public SpiceCollector spiceCollector;
        public CharacterController controller;
        public Animator animatorController;

        public GameHud gameHud;

        public Transform body;
        
        public WheelAxis axisMain;
        public WheelAxis axisFront;
        public WheelAxis axisBack;

        public float baseSpeed;
        public float maxSpeed;
        public float minSpeed;

        public float acceleration;
        public float deceleration;
        public float breakDeceleration;

        [MyBox.ReadOnly]
        public float speed;

        [MyBox.ReadOnly]
        public float rotationSpeed;

        public float baseRotationSpeed;

        public float gravity;

        public float angularSpeedMultiplier = 2.0f;

        public float stopSpeedDetection = 0.01f;

        public bool controlDisabled = false;
        
        private void Awake()
        {
            speed = 0.2f;
        }

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
        
            if (accelerate.IsPressed() && !controlDisabled)
            {
                if (speed > 0)
                {
                    speed += acceleration * dt;
                }
                else
                {
                    speed += (acceleration + breakDeceleration) * dt;
                }
            }
            else if (reverse.IsPressed() && !controlDisabled)
            {
                if (speed > 0)
                {
                    speed -= (acceleration + breakDeceleration) * dt;
                }
                else
                {
                    speed -= acceleration * dt;
                }
            }
            else
            {
                if (speed >= stopSpeedDetection)
                {
                    speed -= deceleration * dt;
                }
                else if (speed <= -stopSpeedDetection)
                {
                    speed += deceleration * dt;
                }
                else
                {
                    speed = 0;
                }
            }

            speed = Mathf.Clamp(speed, minSpeed, maxSpeed);
        
            if (turnLeft.IsPressed() && !controlDisabled)
            {
                // rotationDirection += speed > 0 ? 1f : -1f;
                rotationDirection += 1f;
            }    
        
            if (turnRight.IsPressed() && !controlDisabled)
            {
                // rotationDirection += speed > 0 ? -1f : 1f;
                rotationDirection += -1f;
            }

            rotationSpeed = baseRotationSpeed + speed * angularSpeedMultiplier;
        
            transform.localEulerAngles += new Vector3(0, rotationSpeed * rotationDirection * dt, 0);

            var motion = transform.forward * totalSpeed * dt;
            motion += new Vector3(0, gravity * dt, 0);

            controller.Move(motion);

            if (body != null)
            {
                if (axisMain != null)
                {
                    var eulerAngles = body.localEulerAngles;
                    eulerAngles.x = Vector3.SignedAngle(transform.up, axisMain.up, transform.right);
                    // eulerAngles.x = Vector3.Angle(transform.up, axisMain.up);
                    body.localEulerAngles = eulerAngles;
                }

                if (axisFront != null && axisBack != null)
                {
                    var up = (axisFront.up + axisBack.up) * 0.5f;
                    var eulerAngles = body.localEulerAngles;
                    eulerAngles.z = Vector3.SignedAngle(transform.up, up, transform.forward);
                    body.localEulerAngles = eulerAngles;
                }
            }

            var isMoving = Mathf.Abs(speed) > 0.01f;

            if (spiceCollector != null)
            {
                spiceCollector.isEnabled = !isMoving && !controlDisabled;

                if (animatorController != null)
                {
                    animatorController.SetBool("harvesting", spiceCollector.isHarvesting && !isMoving);
                }

                if (gameHud != null)
                {
                    gameHud.UpdateSpice(spiceCollector.total);
                }
            }
        }
    }
}
