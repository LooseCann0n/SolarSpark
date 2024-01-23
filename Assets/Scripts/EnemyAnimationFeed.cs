using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class EnemyAnimationFeed : MonoBehaviour
{
    public JackalBt jackalBt;
    public AudioSource modulatedSource;
    public AudioSource extraSource;
    public AudioSource movementSource;

    public BoxCollider weaponHitbox;
    public BoxCollider kickHitBox;

    public AudioClip[] servoSounds;
    public AudioClip[] stepSounds;

    public SobekVoice voice;
    private void Start()
    {
        weaponHitbox.GetComponent<Rigidbody>().isKinematic = false;
    }

    public void EnableHitbox()
    {
        weaponHitbox.enabled = true;
    }

    public void DisableHitbox()
    {
        weaponHitbox.enabled = false;
    }

    public void EnableKickHitbox()
    {
        kickHitBox.enabled = true;
    }

    public void DisableKickHitbox()
    {
        kickHitBox.enabled = false;
    }

    public void PlayServoSound()
    {
        movementSource.PlayOneShot(servoSounds[Random.Range(0, servoSounds.Length)], 0.25f);
    }
    
    public void PlayFootstep()
    {
        movementSource.PlayOneShot(stepSounds[Random.Range(0, servoSounds.Length)], 0.25f);
    }

    public void PlayAttack()
    {
        voice.PlayEnemyAttackClip();
    }
    //public void AttackCooldown()
    //{
    //    StartCoroutine(jackalBt.AttackCoroutine());
    //}
}
