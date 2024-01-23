using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviourTree
{

    public class CanAttackNode : Node
    {
        private bool canAttack = true;
        private NavMeshAgent agent;
        private AttackNode attackNode;

        private float weaponCooldownTimer;

        public CanAttackNode(NavMeshAgent agent)
        {
            this.agent = agent;
        }

        public override NodeState EvaluateState()
        {
            if (canAttack)
            {
                agent.isStopped = true;
                return NodeState.SUCCESS;
            }
            if (!canAttack)
            {
                weaponCooldownTimer += Time.deltaTime;
                agent.isStopped = false;
                return NodeState.FAILURE;
            }
            return NodeState.FAILURE;
        }
    }

}

