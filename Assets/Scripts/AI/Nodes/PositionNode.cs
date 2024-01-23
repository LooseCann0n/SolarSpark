using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviourTree
{
    public class PositionNode : Node
    {
        private List<AttackPosition> attackPositions = new List<AttackPosition>();
        private NavMeshAgent enemyAgent;
        private Transform player;
        private Animator animator; 
        private BoxCollider collider;
        private float chaseSpeed;
        private float runDistance;

        private bool assignedAttack;
        private AttackPosition closestAttackPosition;

        public PositionNode(List<AttackPosition> attackPositions, NavMeshAgent agent, Transform player, Animator animator, float chaseSpeed, float runDistance)
        {
            this.attackPositions = attackPositions;
            this.enemyAgent = agent;
            this.player = player;
            this.animator = animator;
            this.chaseSpeed = chaseSpeed;
            this.runDistance = runDistance;
        }

        public override NodeState EvaluateState()
        {
            float shortestDistance = Mathf.Infinity;

            if (attackPositions.Count <= 0)
                return NodeState.FAILURE;

            if (assignedAttack == false)
                for (int i = 0; i < attackPositions.Count; i++)
                {
                    if (attackPositions[i].enemyUsing == null)
                    {
                        // Find the closest attackPosition that hasn't been used
                        float distanceFromTarget = Vector3.Distance(enemyAgent.transform.position, attackPositions[i].transform.position);

                        // If distance is less than shortestDistance
                        if (distanceFromTarget < shortestDistance)
                        {
                            // Assign shortestDistance as distanceFromTarget
                            shortestDistance = distanceFromTarget;
                            closestAttackPosition = attackPositions[i]; // nearestLockOn is target closest to the player
                        }
                    }
                }
            if (closestAttackPosition != null)
            {
                animator.SetBool("Walk", true);
                //closestAttackPosition.enemyUsing = enemyAgent.transform;
                //if (enemyAgent.destination != closestAttackPosition.transform.position)
                //{
                //    enemyAgent.ResetPath();
                //    enemyAgent.SetDestination(closestAttackPosition.transform.position);
                //}

                enemyAgent.SetDestination(closestAttackPosition.transform.position);
                assignedAttack = true;

                if (Vector3.Distance(enemyAgent.transform.position, player.position) > runDistance)
                {
                    enemyAgent.speed = chaseSpeed;
                    animator.SetBool("Run", true);
                }
                if (enemyAgent.remainingDistance <= enemyAgent.stoppingDistance)
                {
                    enemyAgent.speed = 4f;
                    if (!enemyAgent.hasPath || enemyAgent.velocity.sqrMagnitude == 0f)
                    {
                        enemyAgent.speed = 4f;
                        animator.SetBool("Run", false);
                        return NodeState.SUCCESS;
                    }
                }
            }
            if (assignedAttack == false)
            {
                enemyAgent.isStopped = false;
                return NodeState.FAILURE;
            }
            return NodeState.FAILURE;
        }
    }
}
