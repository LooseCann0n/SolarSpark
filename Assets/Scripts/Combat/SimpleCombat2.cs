using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class SimpleCombat2 : MonoBehaviour
{
    [SerializeField]
    AnimationScriptableObject animations;

    Animation playerAnimation;

    Animator playerAnimator;

    [SerializeField]
    List<AnimationClip> lightAttacks;
    [SerializeField]
    List<AnimationClip> heavyAttacks;

    [SerializeField]
    bool canAttack = false;

    private void Start()
    {
        playerAnimation = GetComponentInChildren<Animation>();
        lightAttacks = new List<AnimationClip>();
        heavyAttacks = new List<AnimationClip>();
        RefreshAttacks();
        EnableAttack();
    }

    public void LightAttack(InputAction.CallbackContext context)
    {
        if (lightAttacks.Count <= 0 || canAttack == false)
        {
            return;
        }
        if (context.started)
        {
            Debug.Log("Attack called");
            playerAnimation.AddClip(lightAttacks[0], "light attack");
            //playerAnimation.clip = lightAttacks[0];
            playerAnimation.Play("light attack");
        }
        //canAttack = false;
        //lightAttacks.RemoveAt(0);
    }

    public void HeavyAttack()
    {
        if (heavyAttacks.Count <= 0 || canAttack == false)
        {
            return;
        }
        playerAnimation.clip = heavyAttacks[0];
        playerAnimation.Play();
        canAttack = false;
        heavyAttacks.RemoveAt(0);
    }

    public void EnableAttack()
    {
        canAttack = true;
    }

    public void RefreshAttacks()
    {
        playerAnimation.clip = animations.idleAnimation;
        lightAttacks = animations.lightAttacks;
        heavyAttacks = animations.heavyAttacks;
    }

}
