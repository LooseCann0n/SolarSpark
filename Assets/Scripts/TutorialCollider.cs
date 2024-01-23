using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TutorialCollider : MonoBehaviour
{
    public InputTutorial inputTutorial;
    [Tooltip("What you want to be displayed, type CONTROL and the action button will appear")]
    [TextArea(5, 10)]
    public string displayText;
    [Tooltip("Use the exact name of the action you want to display eg: Jump")]
    public string actionName;
    public string actionDescription;
    [Tooltip("Amount of time the tutorial text will appear")]
    public float textDisplayTime;
    [SerializeField]
    private bool triggered;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player" && !triggered)
        {
            if (actionName == "")
            {
                StartCoroutine(inputTutorial.DisplayInputAction(displayText, actionDescription, textDisplayTime));
                triggered = true;
                return;
            }
            StartCoroutine(inputTutorial.DisplayInputAction(displayText, actionName, actionDescription, textDisplayTime));
            triggered = true;
        }
    }
}

