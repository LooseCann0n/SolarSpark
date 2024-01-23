using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class NextScene : MonoBehaviour
{
    [TextArea(10, 10)]
    public string loadingText; // Text for loading screen
    public SceneField levelToLoad; // Level you want to load
    private Animator introControl; // Animator reference
    public Sprite loadingScreenSprite;

    private void Start()
    {
        introControl = GetComponent<Animator>();
    }

    public void DoNextScene()
    {
        SceneChangeHandler.Instance.LoadLevel(levelToLoad.SceneName, loadingText, loadingScreenSprite);
    }

    public void FadeToBlack()
    {
        introControl.SetTrigger("NextScene");
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame || Gamepad.current.startButton.wasPressedThisFrame)
        {
            FadeToBlack();
        }
    }
}
