using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class PlayerVoice : MonoBehaviour
{
    public AudioSource voiceSource;
    public TextMeshProUGUI subtitleText;


    private void Start()
    {
        subtitleText = GameObject.Find("Subtitles").GetComponent<TextMeshProUGUI>();
    }
    /// <summary>
    /// Use for important dialogue or lines for the player
    /// </summary>
    /// <param name="clipToPlay">This is the clip to be played</param>
    public void PlayClip(DialoguePair clipToPlay)
    {
        if (voiceSource.isPlaying)
        {
            StopAllCoroutines();
            voiceSource.Stop();
            voiceSource.PlayOneShot(clipToPlay.clip);
            StartCoroutine(SubtitleDuration(clipToPlay.clip.length));
            subtitleText.text = clipToPlay.subtitle;

        }
        else
        {
            StopCoroutine(SubtitleDuration(clipToPlay.clip.length));
            voiceSource.PlayOneShot(clipToPlay.clip);
            StartCoroutine(SubtitleDuration(clipToPlay.clip.length));
            subtitleText.text = clipToPlay.subtitle;
        }

    }
    public void PlayClip(AudioClip clipToPlay)
    {
        if (voiceSource.isPlaying)
        {
            StopAllCoroutines();
            voiceSource.Stop();
            voiceSource.PlayOneShot(clipToPlay);

        }
        else
        {
            voiceSource.PlayOneShot(clipToPlay);
        }
    }
    /// <summary>
    /// Use for sounds which aren't important and shouldn't override other dialogue or lines
    /// </summary>
    /// <param name="clipToPlay">This is the clip to be played</param>
    public void PlaySecondaryClip(AudioClip clipToPlay)
    {
        if(!voiceSource.isPlaying)
            voiceSource.PlayOneShot(clipToPlay);
    }

    IEnumerator SubtitleDuration(float clipDuration)
    {
        yield return new WaitForSeconds(clipDuration);
        subtitleText.text = "";

    }
}
