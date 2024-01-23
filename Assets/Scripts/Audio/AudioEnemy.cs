using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEnemy : AudioModulation
{
    public AudioSource extraSource;
    public AudioClip[] enemyDamageClips;
    public AudioClip[] enemyDeathClips;
    public AudioClip[] enemyAlertClips;
    public AudioClip[] enemyDeathChatter;
    public AudioClip[] enemyAttackChatter;
    public AudioClip[] enemyIdleChatter;
    public virtual void PlayEnemyDamageClip()
    {
        targetSource.pitch = Random.Range(0.9f, 1.1f);
        targetSource.PlayOneShot(enemyDamageClips[Random.Range(0, enemyDamageClips.Length)], 4f);
    }
    public void PlayEnemyDeathClip()
    {
        targetSource.pitch = Random.Range(0.9f, 1.1f);
        targetSource.PlayOneShot(enemyDeathClips[Random.Range(0, enemyDeathClips.Length)], 2f);
    }

    public virtual void PlayEnemyAttackClip()
    {
        targetSource.pitch = Random.Range(0.9f, 1.1f);
        targetSource.PlayOneShot(attackClip[Random.Range(0, attackClip.Length)], 1f);
    }

    public virtual void PlayEnemyAlert()
    {
        extraSource.PlayOneShot(enemyAlertClips[Random.Range(0, attackClip.Length)], 0.4f);
    }

    public void PlayEnemyDeathChatter()
    {
        extraSource.PlayOneShot(enemyDeathChatter[Random.Range(0, attackClip.Length)], 1f);
    }
    public virtual void PlayEnemyAttackChatter()
    {
        extraSource.PlayOneShot(enemyAttackChatter[Random.Range(0, attackClip.Length)], 1f);
    }
    public virtual void PlayEnemyIdleChatter()
    {
        extraSource.PlayOneShot(enemyIdleChatter[Random.Range(0, enemyIdleChatter.Length)], 3f);
    }
}
