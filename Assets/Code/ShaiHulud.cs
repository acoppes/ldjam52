using System;
using Gemserk.Gameplay;
using MyBox;
using UnityEngine;

namespace Code
{
    public class ShaiHulud : MonoBehaviour
    {
        public float followTargetSpeed = 15;
        public float followTargetStopDistance = 2.0f;

        public float attackRange = 12.0f;

        public Vector3 attackPosition = new Vector3(0, 3.5f, 0);
        public Vector3 hiddenPosition = new Vector3(0, -20f, 0);
        
        public ParticleSystem outerParticleSystem;
        public ParticleSystem innerParticleSystem;

        public GameObject body;
        public GameObject miniMapIndicator;

        public Harvester harvester;

        public Cooldown startAttackCooldown = new Cooldown(3);
        public Cooldown attackDuration = new Cooldown(4);
        
        public Cooldown spawningFirstCooldown = new Cooldown(5);
        public Cooldown spawningSecondCooldown = new Cooldown(3);
        
        [NonSerialized]
        public States states = new States();

        [ReadOnly()]
        public string currentStates;
        
        private void Awake()
        {
            var outerEmission = outerParticleSystem.emission;
            outerEmission.enabled = false;

            var innerEmission = innerParticleSystem.emission;
            innerEmission.enabled = false;
            
            body.SetActive(false);
            body.transform.localPosition = hiddenPosition;
            
            miniMapIndicator.SetActive(false);

            states.onEnterState += OnEnterState;
            states.onExitState += OnExitState;
            states.EnterState("Idle");
            
            startAttackCooldown.Fill();
        }

        private void OnEnterState(string state)
        {
            if (state.Equals("Idle"))
            {
                // start timer for next attack
                startAttackCooldown.Reset();
            }
            
            if (state.Equals("Follow"))
            {
                miniMapIndicator.SetActive(true);
            }
            
            if (state.Equals("Spawning"))
            {
                var outerEmission = outerParticleSystem.emission;
                outerEmission.enabled = true;
                
                spawningFirstCooldown.Reset();
            }
            
            if (state.Equals("SpawningSecond"))
            {
                var innerEmission = innerParticleSystem.emission;
                innerEmission.enabled = true;
                
                spawningSecondCooldown.Reset();
            }
            
            if (state.Equals("Attack"))
            {
                body.SetActive(true);
                
                LeanTween.moveLocal(body, attackPosition, 3.0f)
                    .setEaseInQuad();
                
                attackDuration.Reset();
                
                startAttackCooldown.Reset();
            }
            
            if (state.Equals("Leave"))
            {
                body.SetActive(true);
                
                LeanTween.moveLocal(body, hiddenPosition, 2.0f)
                    .setEaseInQuad()
                    .setOnCompleteParam(this)
                    .setOnComplete(delegate(object o)
                    {
                        var shaiHulud = o as ShaiHulud;
                        shaiHulud.states.ExitState("Leave");
                        shaiHulud.states.EnterState("Idle");
                    });
            }
            
            currentStates = string.Join(" | ", states.currentStates);
        }
        
        private void OnExitState(string state)
        {
            if (state.Equals("Leave"))
            {
                body.SetActive(false);
                
                var outerEmission = outerParticleSystem.emission;
                outerEmission.enabled = false;

                var innerEmission = innerParticleSystem.emission;
                innerEmission.enabled = false;
                
                miniMapIndicator.SetActive(false);
            }
            
            currentStates = string.Join(" | ", states.currentStates);
        }

        public void Spawn(Vector3 position)
        {
            transform.position = position;
            var emissionModule = outerParticleSystem.emission;
            emissionModule.enabled = true;
        }

        public void Update()
        {
            var dt = Time.deltaTime;
            
            spawningFirstCooldown.Increase(dt);
            spawningSecondCooldown.Increase(dt);
            startAttackCooldown.Increase(dt);
            attackDuration.Increase(dt);
            
            if (states.HasState("Idle"))
            {
                // wait for harvester to harvest some stuff or random time
                if (startAttackCooldown.IsReady && harvester != null && harvester.gameObject.activeInHierarchy)
                {
                    states.ExitState("Idle");
                    states.EnterState("Follow");
                }

                return;
            }
            
            if (states.HasState("Follow"))
            {
                var target = harvester.transform.position;
                target.y = 0;

                var source = transform.position;
                source.y = 0;

                var difference = (target - source);
                
                var direction = difference.normalized;
                var newPosition = transform.position + direction * followTargetSpeed * dt;

                transform.position = newPosition;

                if (difference.sqrMagnitude < followTargetStopDistance * followTargetStopDistance)
                {
                    states.ExitState("Follow");
                    states.EnterState("Spawning");
                }

                return;
            }
            
            if (states.HasState("Spawning"))
            {
                if (spawningFirstCooldown.IsReady)
                {
                    states.ExitState("Spawning");
                    states.EnterState("SpawningSecond");
                }
                // wait for harvester to harvest some stuff or random time

                return;
            }
            
            if (states.HasState("SpawningSecond"))
            {
                if (spawningSecondCooldown.IsReady)
                {
                    states.ExitState("SpawningSecond");
                    states.EnterState("Attack");
                }

                return;
            }

            if (states.HasState("Attack"))
            {
                if (attackDuration.IsReady)
                {
                    states.ExitState("Attack");
                    states.EnterState("Leave");
                }

                return;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color= Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}