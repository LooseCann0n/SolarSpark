using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationFeedForward : MonoBehaviour
{
    private Animator animator;
    public SimpleCombat combat;
    public PlayerManager player;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void ClearAttacks()
    {
        combat.clearAttacks();
    }
    public void EnableAttacks()
    {
        combat.EnableAttacking();
    }
    public void DisableAttacks()
    {
        combat.DisableAttacking();
    }
     public void MovementOn()
    {
        combat.EnableMovement();
    }
    public void MovementOff()
    {
        combat.DisableMovement();
    }
    public void HitboxOn(int damage)
    {
        combat.EnableHitbox();
        combat.playerDamage = damage;
    }

    public void HitboxOff()
    {
        combat.DisableHitbox();
    }
    public void SpecialAttack()
    {
        combat.SpawnSpecialAttack();
    }

    public void ResetHurt()
    {
        animator.ResetTrigger("hurt");
    }
    public void DoHeal()
    {
        player.Heal();
    }

    public void DoHealShake()
    {
        player.HealShake();
    }

    public void ClearBlockTrigger()
    {
        animator.ResetTrigger("parryTrigger");
    }

    public void StartParry()
    {
        combat.StartCoroutine(combat.ParryTime());
    }

    public void ResetDashInt()
    {
        animator.SetInteger("DashType", 0);
    }
}
