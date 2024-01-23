using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class InRangeNode : ActionNode
{
    public float attackRange;

    protected override void OnStart() 
    {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() 
    {
        if (blackboard.playerTransform == null)
            return State.Failure;
        if (Vector3.Distance(context.transform.position, blackboard.playerTransform.position) <= attackRange)
        {
            Debug.Log("In range");
            context.childAnimator.SetBool("Walk", false);
            context.childAnimator.SetBool("Run", false);
            //context.agent.isStopped = true;
            return State.Success;
        }
        return State.Failure;
    }
}
