using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviourTree
{
    public class WanderNode : Node
    {
        private NavMeshAgent enemyAgent;
        private float wanderTimer;
        private float wanderMaxDistance;
        private float wanderSpeed;
        private Animator animator;
        private int chanceToPlay = 10;

        private AudioEnemy audio;

        private Vector3 previousPosition;
        public float curSpeed;

        private float timer;

        public WanderNode(NavMeshAgent enemyAgent, float wanderTimer, float wanderMaxDistance, float wanderSpeed, Animator animator)
        {
            this.enemyAgent = enemyAgent;
            this.wanderTimer = wanderTimer;
            this.wanderMaxDistance = wanderMaxDistance;
            this.wanderSpeed = wanderSpeed;
            this.animator = animator;
            audio = enemyAgent.GetComponent<AudioEnemy>();
        }

        public override NodeState EvaluateState()
        {
            timer += Time.deltaTime;

            float velocity = enemyAgent.velocity.magnitude / enemyAgent.speed;
            animator.SetFloat("Speed", velocity);

            if (timer > wanderTimer)
            {
                Vector3 foundPoint = PickRandomPoint();
                animator.SetBool("Walk", true);
                enemyAgent.SetDestination(foundPoint);
                timer = 0;
                if(Random.Range(0,100) <= chanceToPlay)
                    audio.PlayEnemyIdleChatter();
            }

            return NodeState.RUNNING;
        }

        private Vector3 PickRandomPoint()
        {
            Vector3 randomDirection = Random.insideUnitSphere * wanderMaxDistance;

            randomDirection += enemyAgent.transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, wanderMaxDistance, 1);
            Vector3 finalPosition = hit.position;
            
            return finalPosition;
        }
    }
}

