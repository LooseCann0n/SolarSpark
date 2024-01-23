using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class SetupBlackboard : ActionNode
{
    [SerializeField]
    private Transform player;

    protected override void OnStart() 
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    protected override void OnStop() 
    {
    }

    protected override State OnUpdate() 
    {
        blackboard.playerTransform = player;
        return State.Success;
    }
}
