using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{

    //public GameObject enemyCounter;
    //public TextMeshProUGUI enemiesRemaining;
    public GameObject gameUI;
    //public GameObject winUI;
    public GameObject pauseMenu;
    public PlayerInputActions playerControls;

    private float winnerActiveTimer;
    private GameObject loadingScreen;


    private void Awake()
    {
        playerControls = new PlayerInputActions();
        if (GameObject.Find("LoadingSystem") != null)
            loadingScreen = GameObject.Find("LoadingSystem").transform.GetChild(0).gameObject;
    }


    private void OnEnable()
    {
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void Update()
    {
        if (playerControls.Player.Menu.triggered && loadingScreen.activeInHierarchy == false)
        {
            gameUI.SetActive(!gameUI.activeInHierarchy);
            pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
        }
    }
}
