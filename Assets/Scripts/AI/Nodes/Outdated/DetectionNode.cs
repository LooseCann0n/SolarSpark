using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class DetectionNode : Node
    {
        private Transform player;
        private Transform enemy;
        private float detectionRange;

        public DetectionNode(Transform player, Transform enemy, float detectionRange)
        {
            this.player = player;
            this.enemy = enemy;
            this.detectionRange = detectionRange;
        }

        public override NodeState EvaluateState()
        {
            object currentTarget = GetData("Target");
            if (currentTarget == null)
            {
                float distanceFromPlayer = Vector3.Distance(enemy.position, player.position);
                if (distanceFromPlayer >= detectionRange) // If player is within a certain distnace
                {
                    Debug.Log("Detected player");
                    parent.SetData("Target", player);
                    return NodeState.SUCCESS;
                }
                else
                {
                    return NodeState.FAILURE;
                }
            }
            return NodeState.SUCCESS;
        }
    }
}

