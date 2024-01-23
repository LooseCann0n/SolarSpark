using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace BehaviourTree
{
    public class CirclePlayerNode : Node
    {
        private NavMeshAgent agent;
        private Transform player;
        private Animator animator;
        private List<Transform> pointsAroundPlayer = new List<Transform>();

        private Transform currentTargetPoint;
        private int currentIndexCounter;

        public CirclePlayerNode(NavMeshAgent agent, Transform player, Animator animator, List<Transform> pointsAroundPlayer)
        {
            this.agent = agent;
            this.player = player;
            this.animator = animator;
            this.pointsAroundPlayer = pointsAroundPlayer;
        }

        public override NodeState EvaluateState()
        {
            float shortestDistance = Mathf.Infinity;
            if (currentTargetPoint == null)
            {
                agent.ResetPath();
                for (int i = 0; i < pointsAroundPlayer.Count; i++)
                {
                    // Find the closest attackPosition that hasn't been used
                    float distanceFromPoint = Vector3.Distance(agent.transform.position, pointsAroundPlayer[i].transform.position);

                    // If distance is less than shortestDistance
                    if (distanceFromPoint < shortestDistance)
                    {
                        // Assign shortestDistance as distanceFromTarget
                        shortestDistance = distanceFromPoint;
                        currentTargetPoint = pointsAroundPlayer[i];
                        if (currentIndexCounter == 0)
                            currentIndexCounter = i;
                    }
                }
            }
            if (agent.destination == null)
            {
                agent.SetDestination(currentTargetPoint.position);
                animator.SetBool("Walk", true);
            }

            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    MoveToNextPoint();
                }
            }
            animator.SetBool("Walk", false);
            animator.SetBool("Run", false);
            agent.transform.LookAt(player, Vector3.up);

            return NodeState.RUNNING;
        }

        private void MoveToNextPoint()
        {
            if (currentIndexCounter == pointsAroundPlayer.Count)
                currentIndexCounter = 0;
            agent.isStopped = true;
            pointsAroundPlayer[currentIndexCounter].GetComponent<CirclingSpot>().isCurrentTarget = false;
            currentIndexCounter++;

            if (pointsAroundPlayer[currentIndexCounter].GetComponent<CirclingSpot>().isCurrentTarget == false)
            {
                agent.SetDestination(pointsAroundPlayer[currentIndexCounter].position);
                pointsAroundPlayer[currentIndexCounter].GetComponent<CirclingSpot>().isCurrentTarget = true;

                animator.SetBool("SideStep", true);
                agent.isStopped = false;
            }
            else
            {
                animator.SetBool("SideStep", false);
                agent.isStopped = true;
            }
        }
    }
}


