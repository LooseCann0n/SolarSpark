using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceControl : MonoBehaviour
{
    public AudioSource audioPlayer;
    public AudioLowPassFilter filter;
    [Range(5000f,22000f)]
    public int lowPassMax;
    [Range(0f,10000f)]
    public int lowPassMin;

    private void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
        filter = audioPlayer.gameObject.GetComponent<AudioLowPassFilter>();
    }
    public void LowPassOn()
    {
        filter.cutoffFrequency = lowPassMin;
    }

    public void LowPassOff()
    {
        filter.cutoffFrequency = lowPassMax;
    }
}
