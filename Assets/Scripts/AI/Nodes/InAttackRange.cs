using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviourTree
{
    public class InAttackRangeNode : Node
    {
        private NavMeshAgent enemyAgent;
        private Animator animator;
        private float attackRange;
        private EnemyStance staggerManager;

        public InAttackRangeNode(NavMeshAgent enemyAgent, Animator animator, float attackRange)
        {
            this.enemyAgent = enemyAgent;
            this.animator = animator;
            this.attackRange = attackRange;
            staggerManager = enemyAgent.GetComponent<EnemyStance>();
        }

        public override NodeState EvaluateState()
        {
            if (staggerManager.stunned)
            {
                enemyAgent.isStopped = true;
                return NodeState.FAILURE;
            }
            float velocity = enemyAgent.velocity.magnitude / enemyAgent.speed;

            animator.SetFloat("Speed", velocity);

            object t = (Transform)parent.parent.GetData("Target");
            if (t == null)
            {
                return NodeState.FAILURE;
            }
            Transform target = (Transform)t;
            if (Vector3.Distance(enemyAgent.transform.position, target.position) <= attackRange)
            {
                animator.SetBool("Walk", false);
                animator.SetBool("Run", false);
                enemyAgent.isStopped = true;
                return NodeState.SUCCESS;
            }
            animator.SetBool("Walk", true);
            enemyAgent.isStopped = false;
            return NodeState.FAILURE;
        }
    }

}
