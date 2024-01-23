using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStance : BaseStance
{
    [Tooltip("Speed that stance recovers while the player is blocking")]
    [SerializeField][Range(0,5)] private float blockingStanceRecoverySpeed;

    public UnityEngine.UI.Image stanceBar;
    private SimpleCombat simpleCombat;

    private bool isLerping;

    protected void OnEnable()
    {
        Actions.OnPlayerHit += ResetRecovery;
    }

    protected void OnDisable()
    {
        Actions.OnPlayerHit -= ResetRecovery;
    }

    protected override void Start()
    {
        base.Start();
        stanceBar.fillAmount = 0;
        simpleCombat = GetComponent<SimpleCombat>();
    }

    public override void Stagger(int staggerAmount, float stunTime)
    {
        if (staggerAmount >= stanceBreakpoint) // If stance broken with one attack
        {
            StartCoroutine(AnimateSliderOverTime(0.3f, stanceBreakpoint));
            BrokenStance(1f);
            return;
        }


        StartCoroutine(AnimateSliderOverTime(0.4f, staggerAmount));
        stance += staggerAmount;

        if (stance >= stanceBreakpoint)
            StartCoroutine(BrokenStance(stunTime));
    }

    protected override void Update()
    {
        if (stance > 0)
            recoveryTimer += Time.deltaTime;
        if (recoveryTimer > timeUntilStanceRecovery && stance > 0)
        {
            if (simpleCombat.isBlocking)
                stance -= Time.deltaTime * blockingStanceRecoverySpeed;
            else
                stance -= Time.deltaTime * stanceRecoverySpeed;
            stanceBar.fillAmount = stance / stanceBreakpoint;
            
        }
    }

    protected override IEnumerator BrokenStance(float stunTime)
    {

        stunned = true;
        animator.ResetTrigger("Lattack");
        animator.ResetTrigger("Hattack");
        animator.SetBool("parry", false);
        stanceBar.fillAmount = 0;
        yield return new WaitForSeconds(stunTime);
        stanceBar.fillAmount = 0;
        recoveryTimer = 0;
        stance = 0;
        stunned = false;
    }


    IEnumerator AnimateSliderOverTime(float seconds, float targetValue)
    {
        float currentStance = (float)Stance;
        float animationTime = 0f;
        stanceBar.color = Color.red;
        while (animationTime < seconds)
        {
            animationTime += Time.deltaTime;
            float lerpValue = animationTime / seconds;

            stanceBar.fillAmount = Mathf.Lerp(currentStance, currentStance + targetValue, lerpValue) / (float)stanceBreakpoint;
            yield return null;
        }
        stanceBar.color = Color.black;
    }

    IEnumerator AnimateSliderBackOverTime(float seconds, float targetValue)
    {
        float currentStance = (float)Stance;
        float animationTime = 0f;
        stanceBar.color = Color.red;
        while (animationTime < seconds)
        {
            animationTime += Time.deltaTime;
            float lerpValue = animationTime / seconds;

            stanceBar.fillAmount = Mathf.Lerp(currentStance, targetValue, lerpValue) / (float)stanceBreakpoint;
            yield return null;
        }
        stanceBar.color = Color.black;
    }
}
