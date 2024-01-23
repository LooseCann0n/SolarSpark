using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviourTree
{
    public class ChaseNode : Node
    {
        private NavMeshAgent agent;
        private float waitRange;
        private bool playerEngaged;
        private Animator animator;

        public ChaseNode(NavMeshAgent agent, float chaseRange, bool playerEngaged, Animator animator)
        {
            this.agent = agent;
            this.waitRange = chaseRange;
            this.playerEngaged = playerEngaged;
            this.animator = animator;
        }

        public override NodeState EvaluateState()
        {
            Transform target = (Transform)GetData("Target");

            if (!playerEngaged)
            {
                if (agent.destination == null)
                    agent.SetDestination(target.position);
                if (Vector3.Distance(agent.transform.position, target.position) > waitRange)
                {
                    return NodeState.SUCCESS;
                }
                return NodeState.RUNNING;
            }
            animator.SetBool("Attack", false);
            return NodeState.FAILURE;
        }
    }
}

