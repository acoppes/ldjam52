using System;
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

        private int currentMission;

        private int currentTotalTarget;
        
        public int[] totals = new[] { 1000, 2000, 4000, 10000 };
        public string[] missions;

        public float respawnDistance = 500f;

        private void Start()
        {
            harvesterVirtualCamera.gameObject.SetActive(true);
            shaiHuludVirtualCamera.gameObject.SetActive(false);

            StartCoroutine(ShowNextMission());
            
            shaiHulud.states.onEnterState += OnShaiHuludEnterState;
            shaiHulud.states.onExitState += OnShaiHuludExitState;
            
            shaiHulud.transform.position = TerrainUtils.RandomPositionNearPosition(harvester.transform.position, respawnDistance);

            currentTotalTarget = totals[currentMission];

            var harvesterPosition = harvester.transform.position;
            harvesterPosition.y = TerrainUtils.GetHeightAtPosition(harvesterPosition);
            harvester.transform.position = harvesterPosition;

            gameHud.targetSpice = currentTotalTarget;
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
                    shaiHulud.transform.position = TerrainUtils.
                        RandomPositionNearPosition(harvester.transform.position, respawnDistance);
                }
                else
                {
                    // leantween and show game over
                }

                
            }
        }

        private IEnumerator ShowNextMission()
        {
            yield return new WaitForSeconds(2.0f);
            var mission = string.Format(missions[currentMission], totals[currentMission]);
            gameHud.SetMission(mission);
        }

        private void Update()
        {
            if (harvester != null && harvester.spiceCollector.total > currentTotalTarget)
            {
                currentMission++;
                StartCoroutine(ShowNextMission());

                currentTotalTarget += totals[currentMission];
                gameHud.targetSpice = currentTotalTarget;
            }
        }
    }
}