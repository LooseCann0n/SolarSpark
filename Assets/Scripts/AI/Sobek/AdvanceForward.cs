using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class AdvanceForward : ActionNode
{
    private Transform player;
    private Vector3 midPoint;
    
    [SerializeField] private float timeUntilAdvance;
    [SerializeField] private float timer;

    protected override void OnStart() 
    {
        player = context.playerTransform;
        
    }

    protected override void OnStop() 
    {
    }

    protected override State OnUpdate() 
    {
        if (timer < timeUntilAdvance)
        {
            timer += Time.deltaTime;
            return State.Running;
        }
        else
        {
            midPoint = (context.transform.position + player.position) / 2;
            blackboard.moveToPosition = midPoint;
            timer = 0;
            return State.Success;
        }
    }
}
