using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : AudioModulation
{
    public AudioSource extraSource;
    public PlayerMovement movementScript;
    public AudioClip specialAttackAudio;
    public AudioClip dashAudio;
    public AudioClip sandFootstep;
    public AudioClip stoneFootstep;
    public AudioClip jumpAudio;
    public AudioClip sandLandAudio;
    public AudioClip hardLandAudio;
    public AudioClip healAudio;

    float landSoundCooldown = 0.5f;
    bool canLand = true;

    public void PlaySpecialAttackClip()
    {
        targetSource.pitch = Random.Range(0.9f, 1.1f);
        targetSource.PlayOneShot(specialAttackAudio, 0.2f);
    }
    public void PlayDashClip()
    {
        targetSource.pitch = Random.Range(0.9f, 1.1f);
        targetSource.PlayOneShot(dashAudio, 0.5f);
    }
    public void PlayWeaponClip(AudioClip attackClip)
    {
        targetSource.pitch = Random.Range(0.9f, 1.1f);
        targetSource.PlayOneShot(attackClip, 0.5f);
    }
    public void PlayJumpClip()
    {
        targetSource.pitch = Random.Range(0.9f, 1.1f);
        targetSource.PlayOneShot(jumpAudio, 0.5f);
    }
    public void PlayLandClip()
    {
        if (canLand)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, LayerMask.NameToLayer("Ground")))
            {
                if (hit.collider.gameObject.tag == "Sand")
                {
                    targetSource.pitch = Random.Range(0.9f, 1.1f);
                    targetSource.PlayOneShot(sandLandAudio, 0.1f);
                }
                if (hit.collider.gameObject.tag == "Stone")
                {
                    targetSource.pitch = Random.Range(0.9f, 1.1f);
                    targetSource.PlayOneShot(hardLandAudio, 0.1f);
                }
            }
            canLand = false;
            StartCoroutine(landCooldown());
        }
    }
    public void PlayPlayerFootstep()
    {
        if (movementScript.grounded)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, LayerMask.NameToLayer("Ground")))
            {
                if(hit.collider.gameObject.tag == "Sand")
                {
                    targetSource.pitch = Random.Range(0.9f, 1.1f);
                    targetSource.PlayOneShot(sandFootstep, 0.1f);
                }
                if (hit.collider.gameObject.tag == "Stone")
                {
                    targetSource.pitch = Random.Range(0.9f, 1.1f);
                    targetSource.PlayOneShot(stoneFootstep, 0.1f);
                }
            }

        }
    }
    public void playParamClip(AudioClip clip)
    {  
        extraSource.PlayOneShot(clip);
    }

    public IEnumerator landCooldown()
    {
        yield return new WaitForSeconds(landSoundCooldown);
        canLand = true;
    }
}
