using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TheKiwiCoder;

[System.Serializable]
public class RandomNavmeshPosition : ActionNode
{
    [SerializeField] private float wanderMaxDistance;


    protected override void OnStart() 
    {
    }

    protected override void OnStop() 
    {
    }

    protected override State OnUpdate() 
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderMaxDistance;
        randomDirection += context.agent.transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, wanderMaxDistance, 1);
        Vector3 finalPosition = hit.position;
        blackboard.moveToPosition = finalPosition;
        return State.Success;
    }
}
