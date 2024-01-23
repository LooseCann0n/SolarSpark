using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GraphicSettings : MonoBehaviour
{
    public Toggle fullScreenToggle;
    public Toggle vSyncToggle;

    public ResolutionType[] resolutions;
    public int selectedResolution;

    public TextMeshProUGUI resText;

    private void Start()
    {
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == Screen.width & resolutions[i].height == Screen.height)
            {
                selectedResolution = i;
                resText.text = resolutions[i].width + " x " + resolutions[i].height;
            }
        }

        if (QualitySettings.vSyncCount == 0)
        {
            vSyncToggle.isOn = true;
        }
        else
            vSyncToggle.isOn = false;
    }

    public void ApplyFullScreen()
    {
        if (fullScreenToggle.isOn)
            Screen.fullScreen = true;
        else
            Screen.fullScreen = false;
    }

    public void ApplyVSync()
    {
        if (vSyncToggle.isOn)
            QualitySettings.vSyncCount = 1;
        else
            QualitySettings.vSyncCount = 0;
    }

    public void ResolutionLeft()
    {
        if (selectedResolution > 0)
            selectedResolution--;

        resText.text = resolutions[selectedResolution].width + " x " + resolutions[selectedResolution].height;
        SetResolution();
    }

    public void ResolutionRight()
    {
        if (selectedResolution < resolutions.Length - 1)
            selectedResolution++;

        resText.text = resolutions[selectedResolution].width + " x " + resolutions[selectedResolution].height;
        SetResolution();
    }

    public void SetResolution()
    {
        Screen.SetResolution(resolutions[selectedResolution].width, resolutions[selectedResolution].height, fullScreenToggle.isOn);
    }

}
