using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace BehaviourTree
{
    public class ApproachPlayerNode : Node
    {
        private NavMeshAgent enemyAgent;
        private float waitingDistance;
        private float maxChaseDistance;
        private float runningDistance;
        private Animator animator;

        public float curSpeed;

        public ApproachPlayerNode(NavMeshAgent enemyAgent, float waitingDistance, float maxChaseDistance, Animator animator, float runningDistance)
        {
            this.enemyAgent = enemyAgent;
            this.waitingDistance = waitingDistance;
            this.maxChaseDistance = maxChaseDistance;
            this.animator = animator;
            this.runningDistance = runningDistance;
        }

        public override NodeState EvaluateState()
        {
            Transform target = (Transform)parent.parent.GetData("Target");
            float velocity = enemyAgent.velocity.magnitude;
            animator.SetFloat("Speed", velocity);
            enemyAgent.SetDestination(MoveToTarget(target.position));
            animator.SetBool("Run", false);
            animator.SetBool("Walk", true);

            if (enemyAgent.remainingDistance <= enemyAgent.stoppingDistance)
            {
                if (!enemyAgent.hasPath || enemyAgent.velocity.sqrMagnitude == 0f)
                {
                    return NodeState.SUCCESS;
                }
            }

            if (Vector3.Distance(enemyAgent.transform.position, target.position) < runningDistance)
            {
                enemyAgent.speed = JackalBt.chaseSpeed;
                animator.SetBool("Run", true);
            }
            if (Vector3.Distance(enemyAgent.transform.position, target.position) > maxChaseDistance)
            {
                animator.SetBool("Run", false);
                animator.SetBool("Walk", true);
                enemyAgent.speed = JackalBt.standardSpeed;
                ClearData("Target");
                enemyAgent.isStopped = false;
                return NodeState.FAILURE;
            }
            return NodeState.RUNNING;
        }

        private Vector3 MoveToTarget(Vector3 playerPosition)
        {
            Vector3 movePos = playerPosition;
            movePos = Vector3.MoveTowards(movePos, enemyAgent.transform.position, waitingDistance);
            animator.SetBool("Walk", true);
            return movePos;
        }
    }
}

