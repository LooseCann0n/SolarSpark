using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MainMenu : MonoBehaviour
{
    public SceneField levelToLoad;
    public GameObject sceneSelect;
    public GameObject LoadingScreenSystem;
    [TextArea(10, 10)]
    public string loadingText;
    public Sprite loadingScreenSprite;
    private Animator animator;

    public void Start()
    {
        LoadingScreenHandler();
        animator = GetComponent<Animator>();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void StartGame()
    {
        if (levelToLoad == null)
        {
            Debug.LogWarning("Tutorial level reference not set for StartGame button");
            return;
        }
        animator.SetTrigger("Fade");
    }

    public void SceneSelect()
    {
        gameObject.SetActive(false);
        sceneSelect.SetActive(true);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    public void LoadNextAnimationEvent()
    {
        SceneChangeHandler.Instance.LoadLevel(levelToLoad.SceneName, loadingText, loadingScreenSprite);
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene(7);
    }

    private void LoadingScreenHandler()
    {
        if(GameObject.Find("LoadingSystem") == null)
        {
            GameObject LoadingSystem = Instantiate(LoadingScreenSystem);
            LoadingSystem.name = LoadingScreenSystem.name;
            LoadingSystem.transform.parent = null;
        }
    }
}
