using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class InputMenu : MonoBehaviour
{
    public GameObject selectedButton;
    public EventSystem eventSystem;
    public GameObject[] keyboardMoveDirections;
    public GameObject controllerMove;

    private void Start()
    {
        controllerMove.SetActive(false);
    }

    private void Update()
    {
        if (Keyboard.current.anyKey.wasPressedThisFrame || Mouse.current.press.wasPressedThisFrame && (controllerMove.activeInHierarchy == true))
        {
            Debug.Log("Keyboard/Mouse");
            controllerMove.SetActive(false);
            for (int i = 0; i < keyboardMoveDirections.Length; i++)
            {
                keyboardMoveDirections[i].SetActive(true);
            }
        }
        if (Gamepad.current.aButton.wasPressedThisFrame && controllerMove.activeInHierarchy == false)
        {
            Debug.Log("Gamepad");
            controllerMove.SetActive(true);
            for (int i = 0; i < keyboardMoveDirections.Length; i++)
            {
                keyboardMoveDirections[i].SetActive(false);
            }
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(selectedButton);
        }
    }
}
