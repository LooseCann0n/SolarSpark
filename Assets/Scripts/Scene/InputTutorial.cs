using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class InputTutorial : MonoBehaviour
{
    private PlayerInput playerInput;

    public GameObject tutorialPanel;

    public TextMeshProUGUI textPro;
    public TextMeshProUGUI tutorialDescription;

    public IEnumerator DisplayInputAction(string textToDisplay, string actionName, string actionDescription, float displayTime)
    {
        tutorialPanel.SetActive(true);
        string bindingText = textToDisplay.Replace("CONTROL", "<style=Player/"+actionName+">");
        textPro.text = bindingText;
        tutorialDescription.text = actionDescription;
        yield return new WaitForSeconds(displayTime);
        tutorialPanel.SetActive(false);

    }

    public IEnumerator DisplayInputAction(string textToDisplay, string actionDescription, float displayTime)
    {
        tutorialPanel.SetActive(true);
        textPro.text = textToDisplay;
        tutorialDescription.text = actionDescription;
        yield return new WaitForSeconds(displayTime);
        tutorialPanel.SetActive(false);
    }

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }
}
