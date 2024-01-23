using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStance : MonoBehaviour, IStaggerable
{
    [SerializeField] protected float stance;
    [SerializeField] public int stanceBreakpoint;
    [SerializeField][Range(1f, 5f)] protected float timeUntilStanceRecovery;
    [SerializeField][Range(0.5f, 5f)] protected float stanceRecoverySpeed;
    [SerializeField] protected float stunDuration;
    [SerializeField] public bool stunned;

    protected float recoveryTimer;
    protected Animator animator;

    public float Stance
    {
        get => stance;
        set => stance = value;
    }

    protected virtual void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    protected virtual void Update()
    {
        if (stance > 0)
            recoveryTimer += Time.deltaTime;
        if (recoveryTimer > timeUntilStanceRecovery)
            stance -= Time.deltaTime * stanceRecoverySpeed;
    }

    public virtual void Stagger(int staggerAmount, float stunTime)
    {
        if (stunTime == 0)
            stunTime = stunDuration;

        stance += staggerAmount;

        if (stance >= stanceBreakpoint)
            StartCoroutine(BrokenStance(stunTime));
    }

    protected virtual IEnumerator BrokenStance(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);
    }

    protected void ResetRecovery()
    {
        recoveryTimer = 0;
    }
}
