using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class MoveToCirclePosition : ActionNode
{
    public float speed = 5;
    public float stoppingDistance = 0.1f;
    public bool updateRotation = true;
    public float acceleration = 40.0f;
    public float tolerance = 1.0f;
    private Animator animator;
    private UnityEngine.AI.NavMeshAgent agent;
    private int totalCirclePoints;
    [SerializeField] private bool circlePlayer;

    protected override void OnStart()
    {
        context.agent.stoppingDistance = stoppingDistance;
        context.agent.speed = speed;
        context.agent.destination = blackboard.moveToPosition;
        context.agent.updateRotation = updateRotation;
        context.agent.acceleration = acceleration;
        animator = context.childAnimator;
        agent = context.agent;
        totalCirclePoints = Object.FindObjectOfType<CreateCirclePoints>().circlePoints.Count;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        float velocity = agent.velocity.magnitude / agent.speed;
        animator.SetBool("SideStep", true);
        context.transform.LookAt(blackboard.playerTransform.position);

        if (context.agent.pathPending)
        {
            animator.SetBool("Walk", true);
            context.agent.isStopped = false;
            return State.Running;
        }

        if (context.agent.remainingDistance < tolerance)
        {
            animator.SetFloat("Speed", 0f);
            animator.SetBool("Walk", false);
            SetNextCirclePoint();
            return State.Success;
        }

        if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
        {
            return State.Failure;
        }

        animator.SetFloat("Speed", velocity);
        return State.Running;
    }

    private void SetNextCirclePoint()
    {
        if (blackboard.circleCounter == 0 && blackboard.circleDirection == 1) // First index of circle list
        {
            blackboard.circleCounter = totalCirclePoints - 1; // Set to last index
        }
        else if (blackboard.circleCounter == totalCirclePoints - 1 && blackboard.circleDirection == 0)
        {
            blackboard.circleCounter = 0;

        }
        else if (blackboard.circleDirection == 0)
        {
            blackboard.circleCounter++;
        }
        else
            blackboard.circleCounter--;
    }
}
