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

            StartCoroutine(Intro());
            
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
        }

        private IEnumerator Intro()
        {
            yield return new WaitForSeconds(1.0f);
            gameHud.SetMission("Harvest 1000t of Spice in order to get evacuated and don't get us waiting.");
        }
    }
}