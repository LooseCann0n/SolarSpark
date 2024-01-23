using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SobekVoice : AudioEnemy
{
    public AudioSource sobekVoice;

    public AudioClip[] sobekDamage;

    public void PlaySobekVoice(AudioClip clip)
    {
        if (!sobekVoice.isPlaying)
        {
            sobekVoice.PlayOneShot(clip,5f);
        }
    }
    public virtual void PlayEnemyAttackClip()
    {
        targetSource.pitch = Random.Range(0.9f, 1.1f);
        targetSource.PlayOneShot(attackClip[Random.Range(0, attackClip.Length)], 1f);
        PlayEnemyAttackChatter();
    }
    public override void PlayEnemyDamageClip()
    {
        targetSource.pitch = Random.Range(0.9f, 1.1f);
        targetSource.PlayOneShot(enemyDamageClips[Random.Range(0, enemyDamageClips.Length)], 4f);
        PlaySobekVoice(sobekDamage[Random.Range(0, sobekDamage.Length)]);
    }
    public override void PlayEnemyAlert()
    {
        PlaySobekVoice(enemyAlertClips[Random.Range(0, attackClip.Length)]);
    }
    public virtual void PlayEnemyAttackChatter()
    {
        PlaySobekVoice(enemyAttackChatter[Random.Range(0, attackClip.Length)]);
    }
    public virtual void PlayEnemyIdleChatter()
    {
        PlaySobekVoice(enemyIdleChatter[Random.Range(0, enemyIdleChatter.Length)]);
    }
}
