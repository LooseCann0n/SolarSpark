using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviourTree
{
    public class RandomPosition : Node
    {
        private Transform player;
        private NavMeshAgent agent;
        private float walkRadius;

        private bool foundPath;

        public RandomPosition(Transform player, NavMeshAgent agent, float walkRadius)
        {
            this.player = player;
            this.agent = agent;
            this.walkRadius = walkRadius;
        }

        public override NodeState EvaluateState()
        {
            if(agent.destination == null)
                FindRandom();

            if (agent.destination == null)
                return NodeState.FAILURE;
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    agent.isStopped = true;
                    FindRandom();
                }
            }
            return NodeState.SUCCESS;
        }

        void FindRandom()
        {
            agent.isStopped = false;
            Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
            randomDirection += player.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
            Vector3 finalPosition = hit.position;
            agent.SetDestination(finalPosition);
        }
    }
}
