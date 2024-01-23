using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

public class Screenshotter : MonoBehaviour
{
    public UnityEngine.InputSystem.PlayerInput playerInput;

    public void OnEnable()
    {
        Actions.OnPlayerScreenshot += TakeScreenshot;
    }

    public void OnDisable()
    {
        Actions.OnPlayerScreenshot -= TakeScreenshot;
    }

    private void Start()
    {
        playerInput = GameObject.FindWithTag("Player").GetComponent<UnityEngine.InputSystem.PlayerInput>();
    }

    private void Update()
    {
        if (playerInput.actions["Screenshot"].triggered)
            Actions.OnPlayerScreenshot?.Invoke();
    }

    private void TakeScreenshot()
    {
        ScreenCapture.CaptureScreenshot("Screenshot " + System.DateTime.Now.ToString("MM-dd-yy (HH-mm-ss)") + ".png");
    }
}
