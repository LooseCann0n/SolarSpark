using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class KickAttack : ActionNode
{
    private SimpleCombat simpleCombat;
    [SerializeField]
    private float timeUntilKick;
    private float timer;
    private Animator animator;

    protected override void OnStart() 
    {
        simpleCombat = GameObject.FindWithTag("Player").GetComponent<SimpleCombat>();
        animator = context.childAnimator;
    }

    protected override void OnStop() 
    {
    }

    protected override State OnUpdate() 
    {
        if (simpleCombat.isBlocking == true)
        {
            timer += Time.deltaTime;
        }
        if (simpleCombat.isBlocking == false)
        {
            timer = 0;
            return State.Failure;
        }
        if (timer > timeUntilKick)
        {
            animator.SetTrigger("Kick");
            timer = 0;
            return State.Success;
        }

        return State.Running;
    }
}
