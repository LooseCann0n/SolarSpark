using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{

    public string mixerName;
    public string volumeStringVal;
    public Slider volumeSlider;
    public TextMeshProUGUI display;
    public AudioMixer audioMixer;

    public void Start()
    {
        UpdateVolume();
    }

    public void UpdateVolume()
    {
        volumeStringVal = Mathf.Round(volumeSlider.value*100).ToString();
        display.text = mixerName + ": " + volumeStringVal;
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volumeSlider.value) * 20);
    }
}
