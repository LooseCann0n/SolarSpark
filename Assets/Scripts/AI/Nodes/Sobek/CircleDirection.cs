using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TheKiwiCoder;

[System.Serializable]
public class CircleDirection : ActionNode
{
    [SerializeField]
    private int counter;

    public int lowestTime;
    public int highestTime;

    [SerializeField]
    private float timeUntilChange;

    [SerializeField]
    private float timer;

    protected override void OnStart() 
    {
        if (timer >= timeUntilChange)
        {
            counter = Random.Range(0, 2);
            timeUntilChange = Random.Range(lowestTime, highestTime);
            timer = 0;
        }

    }

    protected override void OnStop() 
    {
    }

    protected override State OnUpdate() 
    {
        if (timer < timeUntilChange)
            timer++;

        if (counter == 0)
        {
            blackboard.circleDirection = counter;
            context.childAnimator.SetBool("ClockWise", true);
        }
        if (counter == 1)
        {
            blackboard.circleDirection = counter;
            context.childAnimator.SetBool("ClockWise", false);
        }
        return State.Success;
    }
}
