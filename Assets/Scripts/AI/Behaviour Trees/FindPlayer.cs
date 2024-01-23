using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class FindPlayer : ActionNode
{
    [SerializeField]
    private Transform enemyTransform;
    [Range(30, 360)]
    public float viewAngle;
    [Range(1, 20)]
    public float fovRange;
    [Range(5, 25)]
    public float maxChaseDistance;
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    protected override void OnStart()
    {
        enemyTransform = context.transform;
    }
    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (blackboard.playerTransform == null) // If there is no currentTarget
        {
            Collider[] colliders = Physics.OverlapSphere(enemyTransform.position, fovRange, targetMask); // Sphere of all targets in fovRange

            // Standard FOV detection
            if (colliders.Length > 0) // If 1 or more colliders are detected
            {
                Transform target = colliders[0].transform; // Set local transform target to first collider
                Debug.Log(target.transform);
                Vector3 dirToTarget = (target.position - enemyTransform.position).normalized; // Find the angle to the target
                if (Vector3.Angle(enemyTransform.forward, dirToTarget) < viewAngle / 2) // If valid angle found
                {
                    Debug.DrawLine(enemyTransform.position, target.position, Color.red);
                    float distToTarget = Vector3.Distance(enemyTransform.position, target.position); // Find total distance to the target
                    if (Physics.Raycast(enemyTransform.position, dirToTarget, distToTarget, targetMask)) // If there LOS from the enemy to the player
                    {
                        blackboard.playerTransform = target;
                        return State.Success; // Return SUCCESS
                    }
                }
            }
            return State.Failure; // No targets found so return FAILURE
        }
        if (Vector3.Distance(enemyTransform.transform.position, blackboard.playerTransform.position) >= maxChaseDistance)
            ClearTarget();
        return State.Success; // Already target so return SUCCESS
    }

    private void ClearTarget()
    {
        blackboard.playerTransform = null;
    }
}

