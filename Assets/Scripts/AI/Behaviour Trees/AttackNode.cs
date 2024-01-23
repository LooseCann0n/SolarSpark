using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TheKiwiCoder;

[System.Serializable]
public class AttackNode : ActionNode
{
    public float slashCooldown;
    public float spinAttackCooldown;

    private bool slashReady;
    private bool spinReady;

    private float slashTimer;
    private float spinTimer;

    private bool attackReady;
    private NavMeshAgent agent;

    [SerializeField]
    private float cooldownTimer;

    public enum EnemyType
    {
        Sword,
        Axe,
        Spear
    }
    public EnemyType enemyType;


    private Animator enemyAnimator;

    protected override void OnStart()
    {
        enemyAnimator = context.childAnimator;
        agent = context.agent;
        attackReady = true;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() 
    {
        if (attackReady == false)
        {
            AttackCooldown(slashCooldown);
            return State.Running;
        }
        if (attackReady == true)
        {
            Attack(enemyType);
            return State.Running;
        }
        return State.Running;


        //if (slashReady)
        //    AttackCooldown(slashCooldown, slashReady);
        //if (spinReady)
        //    AttackCooldown(spinAttackCooldown, spinReady);
        //enemyAnimator.SetTrigger("Attack");

    }

    //private IEnumerator AttackCooldown(float attackTime)
    //{
    //    Debug.Log("Attack cooldown Start");
    //    attackReady = false;
    //    agent.isStopped = true;
    //    yield return new WaitForSeconds(attackTime);
    //    agent.isStopped = false;
    //    attackReady = true;
    //    Debug.Log("Attack cooldown End");

    //}

    private void AttackCooldown(float cooldownTime)
    {
        Debug.Log("Attack cooldown Start");
        attackReady = false;
        agent.isStopped = true;
        if (cooldownTimer < cooldownTime)
            cooldownTimer += Time.deltaTime;
        else
        {
            agent.isStopped = false;
            attackReady = true;
            cooldownTimer = 0;
            Debug.Log("Attack cooldown End");
            return;
        }

    }

    private void Attack(EnemyType type)
    {
        attackReady = false;
        //context.transform.LookAt(context.playerTransform);
        if (type == EnemyType.Sword)
            enemyAnimator.SetTrigger("Attack");
        if (type == EnemyType.Axe)
        {
            int selectedAttack = Random.Range(1, 4);
            context.childAnimator.Play("Slash");
            enemyAnimator.SetTrigger("Attack " + selectedAttack);
        }
    }
}
