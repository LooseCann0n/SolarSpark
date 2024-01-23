using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TMPro;
using Cinemachine;

public class SceneChangeHandler : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject panel;
    [SerializeField] private Slider loadSlider;
    [SerializeField] private TextMeshProUGUI progressTxt;
    [SerializeField] private Button continueButton;
    [SerializeField] private LoadingScreenText loadingScreenText;
    private PlayerInput controls;
    private bool canContinue = false;
    private CinemachineInputProvider cameraInput;
    private GameObject playerUI;

    public static SceneChangeHandler Instance;


    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        loadingScreenText = GetComponent<LoadingScreenText>();
    }

    public void LoadLevel(string sceneName, string loadingText, Sprite loadingScreenSprite)
    {

         StartCoroutine(LoadAsync(sceneName, loadingText, loadingScreenSprite));
    }

    private IEnumerator LoadAsync(string sceneName, string loadingScreenLore, Sprite loadingScreenSprite)
    {
        if (sceneName != "IntroScreen") // Ensure loading screen panel doesn't popup for intro screen level
        {
            loadingScreen.SetActive(true);
            loadingScreenText.StartCoroutine(loadingScreenText.DisplayText(loadingScreenLore));
        }
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        panel.GetComponent<Image>().sprite = loadingScreenSprite;


        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            //loadSlider.value = progress;
            //progressTxt.text = progress * 100f + "%";

            yield return null;
        }
        canContinue = true;

        GameObject player = GameObject.FindWithTag("Player");
        controls = player.GetComponent<PlayerInput>();

        controls.DeactivateInput();
        cameraInput = GameObject.FindGameObjectWithTag("Vcam").GetComponent<CinemachineVirtualCamera>().GetComponent<CinemachineInputProvider>();
        playerUI = GameObject.FindWithTag("SunUI");
        playerUI.SetActive(false);
        cameraInput.enabled = false;

        continueButton.interactable = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(continueButton.gameObject);
    }
    public void Continue()
    {
        if (canContinue)
        {
            if (GameObject.FindWithTag("MovementTutorial") != null)
                GameObject.FindWithTag("MovementTutorial").GetComponent<BoxCollider>().enabled = true;
            controls.ActivateInput();
            cameraInput.enabled = true;
            loadingScreen.SetActive(false);
            playerUI.SetActive(true);
            canContinue = false;
            loadSlider.value = 0.00f;
            continueButton.interactable = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
        }
    }

}
