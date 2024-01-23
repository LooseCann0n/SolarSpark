using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class PauseMenu : MonoBehaviour
{
    [Header("Menu Elements")]
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject debugToolsUI;
    public GameObject controlsMenuUI;
    public GameObject debugMenu;
    public GameObject checkPointsUI;
    public GameObject resumeButton;

    [Header("Extra References")]
    public CinemachineInputProvider cameraInput;
    public SceneField sceneToLoad;

    private void Awake()
    {
        cameraInput = GameObject.FindWithTag("Vcam").GetComponent<CinemachineInputProvider>();
    }

    public void OnEnable()
    {
        cameraInput.enabled = false;
        pauseMenu.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(resumeButton);
    }

    public void OnDisable()
    {
        cameraInput.enabled = true;
        optionsMenu.SetActive(false);
        debugToolsUI.SetActive(false);
        controlsMenuUI.SetActive(false);
        debugMenu.SetActive(true);
        checkPointsUI.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }

    public void OptionsMenu()
    {
        optionsMenu.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void ResumeGame()
    {
        gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    public void DebugTools()
    {
        pauseMenu.SetActive(false);
        debugToolsUI.SetActive(true);
    }

    public void ChangeScene(string priorityScene)
    {
        Debug.Log(priorityScene);
        Scene scene = SceneManager.GetActiveScene();
        if (priorityScene == "")
            if (scene.name != sceneToLoad)
                SceneManager.LoadScene(sceneToLoad);
        if (priorityScene != "")
            {
                Debug.Log("Load priority scene");
                SceneManager.LoadScene(priorityScene);
            }
    }

    public void RestartScene()
    {
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }

}
