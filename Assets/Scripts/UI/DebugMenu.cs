using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject debugTools;
    public GameObject checkPoints;

    public void BackButton()
    {
        pauseMenu.SetActive(true);
        debugTools.SetActive(false);
    }

    public void CheckPointButton()
    {
        gameObject.SetActive(false);
        checkPoints.SetActive(true);
    }

}

