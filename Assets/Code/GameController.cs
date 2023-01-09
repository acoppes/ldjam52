using System.Collections;
using Cinemachine;
using UnityEngine;

namespace Code
{
    public class GameController : MonoBehaviour
    {
        public Harvester harvester;
        public ShaiHulud shaiHulud;

        public CinemachineVirtualCamera harvesterVirtualCamera;
        public CinemachineVirtualCamera shaiHuludVirtualCamera;

        public GameHud gameHud;

        private void Start()
        {
            harvesterVirtualCamera.gameObject.SetActive(true);
            shaiHuludVirtualCamera.gameObject.SetActive(false);

            // StartCoroutine(GameLogic());
            
            shaiHulud.states.onEnterState += OnShaiHuludEnterState;
            shaiHulud.states.onExitState += OnShaiHuludExitState;
        }

        private void OnShaiHuludEnterState(string state)
        {
            if (state.Equals("Attack"))
            {
                harvesterVirtualCamera.gameObject.SetActive(false);
                shaiHuludVirtualCamera.gameObject.SetActive(true);
                harvester.controlDisabled = true;
            }
            
            if (state.Equals("Leave"))
            {
                // check harvester distance, if in range of death, then deactivate and game over

                if (Vector3.Distance(harvester.transform.position, shaiHulud.transform.position) <
                    shaiHulud.attackRange)
                {
                    // disable harvester
                    harvester.gameObject.SetActive(false);

                    if (gameHud != null)
                    {
                        gameHud.gameObject.SetActive(false);
                    }
                } 
                
                if (harvester.gameObject.activeInHierarchy)
                {
                    harvesterVirtualCamera.gameObject.SetActive(true);
                    shaiHuludVirtualCamera.gameObject.SetActive(false);
                    harvester.controlDisabled = false;
                }
            }
        }
        
        private void OnShaiHuludExitState(string state)
        {
            if (state.Equals("Leave"))
            {
                // if harvester is alive
                if (harvester.gameObject.activeInHierarchy)
                {
                    shaiHulud.transform.position = TerrainUtils.RandomPositionInsideTerrain();
                }
                else
                {
                    // leantween and show game over
                }

                
            }
            
            // if (state.Equals("Leave"))
            // {
            //     // // if harvester is alive
            //     // if (harvester.gameObject.activeInHierarchy)
            //     // {
            //     //     harvesterVirtualCamera.gameObject.SetActive(true);
            //     //     shaiHuludVirtualCamera.gameObject.SetActive(false);
            //     //     harvester.controlDisabled = false;
            //     // }
            //     // else
            //     // {
            //     //     // leantween and show game over
            //     // }
            //
            //     shaiHulud.transform.position = TerrainUtils.RandomPositionInsideTerrain();
            // }
        }

        private IEnumerator GameLogic()
        {
            while (true)
            {
                       
                
                yield return null;
            }
        }

        public void Update()
        {
            // if (Keyboard.current.digit1Key.wasPressedThisFrame)
            // {
            //     // shaiHulud.
            //     var noise = harvesterVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            //     noise.m_AmplitudeGain = 0.25f;
            //     noise.m_FrequencyGain = 1;
            // }
            //
            // if (Keyboard.current.digit2Key.wasPressedThisFrame)
            // {
            //     // shaiHulud.
            //     var noise = harvesterVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            //     noise.m_AmplitudeGain = 0.5f;
            //     noise.m_FrequencyGain = 1.5f;
            // }
            //
            // if (Keyboard.current.digit3Key.wasPressedThisFrame)
            // {
            //     var noise = harvesterVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            //     noise.m_AmplitudeGain = 0.1f;
            //     noise.m_FrequencyGain = 1.0f;
            //
            //     shaiHulud.Spawn(harvester.transform.position);
            // }
        }
    }
}