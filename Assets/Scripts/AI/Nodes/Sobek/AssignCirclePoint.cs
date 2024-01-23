using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class AssignCirclePoint : ActionNode
{
    private List<Transform> circlePoints;
    private int counter;


    protected override void OnStart() 
    {
        circlePoints = GameObject.Find("AttackPosition").GetComponentInChildren<CreateCirclePoints>().circlePoints;
        blackboard.playerTransform = GameObject.FindWithTag("Player").transform;
    }

    protected override void OnStop() 
    {
    }

    protected override State OnUpdate() 
    {
        blackboard.moveToPosition = circlePoints[blackboard.circleCounter].position;
        context.agent.isStopped = false;
        //context.childAnimator.Play("Side Stepping");
        return State.Success;
    }
}
