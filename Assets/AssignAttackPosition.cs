using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class AssignAttackPosition : ActionNode
{
    private UnityEngine.AI.NavMeshAgent agent;
    private Transform enemyPosition;
    public float stopRange;
    public float maxAggroRange;

    public float radiusAroundTarget = 0.5f;

    private AIManager manager;

    protected override void OnStart()
    {
        agent = context.agent;
        enemyPosition = context.transform;
        manager = AIManager.Instance;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (Vector3.Distance(enemyPosition.position, blackboard.playerTransform.position) > stopRange)
        {
            for (int i = 0; i < manager.enemies.Count; i++)
            {
                manager.enemies[i].GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(new Vector3(
                    blackboard.playerTransform.position.x + radiusAroundTarget * Mathf.Sin(2 * Mathf.PI * i / manager.enemies.Count),
                    blackboard.playerTransform.position.y,
                    blackboard.playerTransform.position.z + radiusAroundTarget * Mathf.Sin(2 * Mathf.PI * i / manager.enemies.Count)));

            }
            blackboard.attackPosition = blackboard.playerTransform;
            return State.Success;
        }
        if (Vector3.Distance(enemyPosition.position, blackboard.playerTransform.position) < maxAggroRange)
        {
            blackboard.attackPosition = null;
            return State.Failure;
        }
        return State.Running;
    }
}
