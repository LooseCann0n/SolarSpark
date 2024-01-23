using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviourTree
{
    public class CheckSpaceNode : Node
    {
        private AttackPosition[] attackPositions;
        private NavMeshAgent agent;

        private AttackPosition closestAttackPosition;

        public CheckSpaceNode(AttackPosition[] attackPositions, NavMeshAgent agent)
        {
            this.attackPositions = attackPositions;
            this.agent = agent;
        }

        public override NodeState EvaluateState()
        {
            float shortestDistance = Mathf.Infinity;

            for (int i = 0; i < attackPositions.Length; i++)
            {
                if (attackPositions[i].enemyUsing == null)
                {
                    // Find the closest attackPosition that hasn't been used
                    float distanceFromTarget = Vector3.Distance(agent.transform.position, attackPositions[i].transform.position);

                    // If distance is less than shortestDistance
                    if (distanceFromTarget < shortestDistance)
                    {
                        // Assign shortestDistance as distanceFromTarget
                        shortestDistance = distanceFromTarget;
                        closestAttackPosition = attackPositions[i]; // nearestLockOn is target closest to the player
                    }
                }
            }           
            if (closestAttackPosition == null)
                return NodeState.FAILURE;
            return NodeState.SUCCESS;
        }
    }
}