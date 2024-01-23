using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.EventSystems;

public class OptionsMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject controlsMenu;

    public void BackButton()
    {
        pauseMenu.SetActive(true);
        gameObject.SetActive(false);
    }
    public void ControlsButton()
    {
        controlsMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
