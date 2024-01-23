using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class FindAttackers : ActionNode
{
    public AIManager aiManager;
    public int attackerCount;

    protected override void OnStart() 
    {
        aiManager = GameObject.FindWithTag("AIManager").GetComponent<AIManager>();
    }

    protected override void OnStop() 
    {
    }

    protected override State OnUpdate() 
    {
        if (aiManager.enemies.Contains(context.transform))
        {
            return State.Success;
        }
        if (aiManager.AttackSpotsAvailable())
        {
            aiManager.activeAttackers++;
            aiManager.enemies.Add(context.transform);
            return State.Success;
        }        
        return State.Failure;
    }
}
