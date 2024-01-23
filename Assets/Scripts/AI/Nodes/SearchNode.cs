using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviourTree
{
    public class SearchNode : Node
    {
        private Transform enemyTransform;
        private float fovRange;
        private float viewAngle;
        private LayerMask targetMask;
        private LayerMask obstacleMask;
        private Transform player;
        private Animator animator;
        private AudioEnemy audio;


        public SearchNode(Transform enemyTransform, float fovRange, float viewAngle, LayerMask targetMask, LayerMask obstacleMask, Animator animator)
        {
            this.enemyTransform = enemyTransform;
            this.fovRange = fovRange;
            this.viewAngle = viewAngle;
            this.targetMask = targetMask;
            this.obstacleMask = obstacleMask;
            this.animator = animator;
            audio = enemyTransform.GetComponent<AudioEnemy>();
            player = GameObject.FindWithTag("Player").transform;
            
            Actions.OnPlayerDeath += ClearTarget;

        }

        public override NodeState EvaluateState()
        {          
            object currentTarget = GetData("Target"); // Check if a target has already been found
            if (currentTarget == null) // If there is no currentTarget
            {
                Collider[] colliders = Physics.OverlapSphere(enemyTransform.position, fovRange, targetMask); // Sphere of all targets in fovRange
                
                // Standard FOV detection
                if (colliders.Length > 0) // If 1 or more colliders are detected
                {
                    Transform target = colliders[0].transform; // Set local transform target to first collider
                    Vector3 dirToTarget = (target.position - enemyTransform.position).normalized; // Find the angle to the target
                    if (Vector3.Angle(enemyTransform.forward, dirToTarget) < viewAngle / 2) // If valid angle found
                    {
                        float distToTarget = Vector3.Distance(enemyTransform.position, target.position); // Find total distance to the target
                        if (!Physics.Raycast(enemyTransform.position, dirToTarget, distToTarget, obstacleMask)) // If there LOS from the enemy to the player
                        {
                            parent.parent.SetData("Target", colliders[0].transform); // SetData in 2 nodes above to found Target
                            audio.PlayEnemyAlert();
                            return NodeState.SUCCESS; // Return SUCCESS
                        }
                    }
                }
                return NodeState.FAILURE; // No targets found so return FAILURE
            }
            return NodeState.SUCCESS; // Already target so return SUCCESS
        }

        private void ClearTarget()
        {
            parent.parent.ClearData("Target");
        }
    }

}

