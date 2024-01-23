using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class StayOnPosition : ActionNode
{
    private Animator animator;
    public float speed = 5;
    public float stoppingDistance = 0.1f;
    public bool updateRotation = true;
    public float acceleration = 40.0f;
    public float tolerance = 1.0f;

    protected override void OnStart() 
    {
        animator = context.childAnimator;
        context.agent.stoppingDistance = stoppingDistance;
        context.agent.speed = speed;
        context.agent.destination = blackboard.moveToPosition;
        context.agent.updateRotation = updateRotation;
        context.agent.acceleration = acceleration;
        animator = context.childAnimator;
    }

    protected override void OnStop() 
    {
    }

    protected override State OnUpdate() 
    {
        context.agent.SetDestination(blackboard.attackPosition.position);
        float velocity = context.agent.velocity.magnitude / context.agent.speed;
        animator.SetFloat("Speed", velocity);
        animator.SetBool("Run", true);

        if (context.agent.pathPending)
        {
            animator.SetBool("Walk", true);
            context.agent.isStopped = false;
            return State.Running;
        }
        if (context.agent.remainingDistance < 2f)
        {
            Debug.Log("STOP MOVING");
            animator.SetFloat("Speed", 0f);
            animator.SetBool("Walk", false);
            return State.Success;
        }
        if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
        {
            return State.Failure;
        }
        return State.Running;
    }
}
