using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [Range(0,1)]
    public float targetVolume;
    public float fadeDuration;
    public GameObject calmMusicObject;
    public GameObject aggressiveMusicObject;
    public static MusicController musicInstance;
    AudioSource calmSource;
    AudioSource aggressiveSource;
    bool canChangeMusic = true;

    private void Start()
    {
        calmSource = calmMusicObject.GetComponent<AudioSource>();
        aggressiveSource = aggressiveMusicObject.GetComponent<AudioSource>();
        StartCoroutine(StartFade(calmSource, 10, targetVolume));
        musicInstance = this;
    }

    public IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        canChangeMusic = false;
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        canChangeMusic = true;
        yield break;
    }

    public void StartCombat()
    {
        if (canChangeMusic)
        {
            StartCoroutine(StartFade(aggressiveSource, fadeDuration, targetVolume));
            StartCoroutine(StartFade(calmSource, fadeDuration, 0));
        }
    }

    public void EndCombat()
    {
        if (canChangeMusic)
        {
            StartCoroutine(StartFade(calmSource, fadeDuration, targetVolume));
            StartCoroutine(StartFade(aggressiveSource, fadeDuration, 0));
        }
    }

}
