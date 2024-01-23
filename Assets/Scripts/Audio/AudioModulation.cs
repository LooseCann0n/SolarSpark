using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioModulation : MonoBehaviour
{
    public AudioSource targetSource;
    public AudioClip[] walkClip;
    public AudioClip[] attackClip;
    private void Start()
    {
        if(GetComponent<AudioSource>() != null)
            targetSource = GetComponent<AudioSource>();
    }

    public void PlayWalkClip()
    {
        targetSource.pitch = Random.Range(0.9f, 1.1f);
        targetSource.PlayOneShot(walkClip[Random.Range(0, walkClip.Length)], 0.5f);
    }
    
    public void PlaySelectedClip(AudioClip clip)
    {
        targetSource.PlayOneShot(clip);
    }
    
}
