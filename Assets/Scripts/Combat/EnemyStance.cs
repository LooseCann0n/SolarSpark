using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStance : BaseStance
{
    protected void OnEnable()
    {
        Actions.OnEnemyHit += ResetRecovery;
    }

    protected void OnDisable()
    {
        Actions.OnEnemyHit -= ResetRecovery;
    }

    protected override IEnumerator BrokenStance(float stunTime)
    {
        stunned = true;
        animator.SetTrigger("Hit 1");
        animator.ResetTrigger("Attack");
        yield return new WaitForSeconds(stunTime);
        recoveryTimer = 0;
        stance = 0;
        stunned = false;
    }
}
