using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class FindClosestCirclePoint : ActionNode
{
    private Transform enemy;
    private List<Transform> circlePoints;
    private Transform closestPoint;
    public int counter;

    protected override void OnStart() 
    {
        enemy = context.transform;
        circlePoints = GameObject.Find("AttackPosition").GetComponentInChildren<CreateCirclePoints>().circlePoints;
        float pointDistance = Mathf.Infinity;
        for (int i = 0; i < circlePoints.Count; i++)
        {
            if (Vector3.Distance(enemy.position, circlePoints[i].position) < pointDistance)
            {
                pointDistance = Vector3.Distance(enemy.position, circlePoints[i].position);
                closestPoint = circlePoints[i];
                counter = i;
            }
        }
        blackboard.circleCounter = counter;
        blackboard.moveToPosition = closestPoint.position;
    }

    protected override void OnStop() 
    {
    }

    protected override State OnUpdate() 
    {
        if (blackboard.moveToPosition == null)
            return State.Failure;
        return State.Success;
    }
}
