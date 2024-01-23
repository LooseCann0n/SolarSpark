using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class II_Player : MonoBehaviour
{
    //Events which we can invoke when actions in the Input Action Assets are activated.
    public event UnityAction MovementEvent = delegate{ };
    public event UnityAction RotationEvent = delegate{ };
    public event UnityAction AttackEvent = delegate{ };
    public event UnityAction JumpEvent = delegate{ };
    public event UnityAction WeaponSwitchEvent = delegate{ };
    public event UnityAction TogglePauseEvent = delegate{ };

    public event UnityAction DeviceLostEvent = delegate{ };
    public event UnityAction DeviceRegainedEvent = delegate{ };

    public event UnityAction ControlsChangedEvent = delegate{ };


    [Header("Input Settings")]
    public PlayerInput playerInput;
    private Vector3 rawInputMovement;
    private Vector3 rawInputRotation;
    private Vector2 rawInputScroll;

    //Action Maps
    private string actionMapPlatformerControls = "Platformer Controls";
    private string actionMapHelicopterControls = "Helicopter Controls";
    private string actionMapMenuControls = "Menu Controls";
    private string actionMapBeforeMenuOpened = "";

    //Current Control Scheme
    private string currentControlScheme;

    private void Awake()
    {
        Time.timeScale = 1;
        if (playerInput == null)
        {
            playerInput = FindObjectOfType<PlayerInput>();
        }
    }

    private void OnEnable()
    {
        if (playerInput == null)
        {
            playerInput = FindObjectOfType<PlayerInput>();
        }

        II_Menu.onMainMenuOpened += HandleMainMenuOpened;
        II_Menu.onMainMenuClosed += HandleMainMenuClosed;

        OnDeviceRegained();
    }

    private void OnDisable()
    {
        II_Menu.onMainMenuOpened -= HandleMainMenuOpened;
        II_Menu.onMainMenuClosed -= HandleMainMenuClosed;
    }

    public void SetInputActiveState(bool gameIsPaused)
    {
        switch (gameIsPaused)
        {
            case true:
                playerInput.DeactivateInput();
                break;

            case false:
                playerInput.ActivateInput();
                break;
        }
    }

    public Vector3 GetInputDirection()
    {
        return rawInputMovement;
    }

    public Vector3 GetRotationDirection()
    {
        return rawInputRotation;
    }

    public Vector2 GetScrollInput()
    {
        return rawInputScroll;
    }


    //INPUT SYSTEM ACTION METHODS --------------
    //This is called from PlayerInput; when a stick or arrow keys has been pushed.
    //It stores the input Vector as a Vector3 to then be used by the smoothing function.
    public void OnMovement(InputAction.CallbackContext value)
    {
        Vector2 inputMovement = value.ReadValue<Vector2>();
        rawInputMovement = new Vector3(inputMovement.x, inputMovement.y, 0);
        MovementEvent?.Invoke();
    }

    public void OnRotation(InputAction.CallbackContext value)
    {
        Vector2 rotationInput = value.ReadValue<Vector2>();
        rawInputRotation = new Vector3(0,0, -rotationInput.x);
        RotationEvent?.Invoke();
    }

    //This is called from PlayerInput, when a button has been pushed, that corresponds with the 'Attack' action
    public void OnAttack(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            AttackEvent?.Invoke();
        }
    }

    //This is called from PlayerInput, when a button has been pushed, that corresponds with the 'Jump' action
    public void OnJump(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            JumpEvent?.Invoke();
        }
    }

    //This is called from PlayerInput, when a button has been pushed, that corresponds with the 'Weapon Switch' action
    public void OnWeaponSwitch(InputAction.CallbackContext value)
    {
       
        if (value.started)
        {
            Vector2 scrollInput = value.ReadValue<Vector2>();
            rawInputScroll = new Vector2(scrollInput.x, scrollInput.y);

            WeaponSwitchEvent?.Invoke();
        }
    }

    //This is called from Player Input, when a button has been pushed, that corresponds with the 'TogglePause' action
    public void OnTogglePause(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            actionMapBeforeMenuOpened = playerInput.currentActionMap.name;
            TogglePauseEvent?.Invoke();
            II_Menu.HandleMenuButtonPressed();
        }
    }

    public void OnCancel(InputAction.CallbackContext value)
    {
        //Debug.Log("Cancel pressed");
    }

    public void OnSubmit(InputAction.CallbackContext value)
    {
        //Debug.Log("Submit pressed");
    }

    public void HandleMainMenuOpened()
    {
        Time.timeScale = 0;
    }

    public void HandleMainMenuClosed()
    {
        Time.timeScale = 1;
    }

    //INPUT SYSTEM AUTOMATIC CALLBACKS --------------

    //This is automatically called from PlayerInput, when the input device has changed
    //(IE: from Keyboard to Xbox Controller)
    public void OnControlsChanged()
    {
        if (playerInput == null)
            return;

        currentControlScheme = playerInput.currentControlScheme;
        ControlsChangedEvent?.Invoke();
    }


    //This is automatically called from PlayerInput, when the input device has been disconnected and can not be identified
    //IE: Device unplugged or has run out of batteries

    public void OnDeviceLost()
    {
        DeviceLostEvent?.Invoke();
    }


    public void OnDeviceRegained()
    {
        if (this.gameObject.activeSelf == false)
            return;

        StartCoroutine(WaitForDeviceToBeRegained());
    }

    IEnumerator WaitForDeviceToBeRegained()
    {
        yield return new WaitForSeconds(0.1f);
        DeviceRegainedEvent?.Invoke();
    }


    //Switching Action Maps ----
    public void EnablePlayerControls(string controlName)
    {
        playerInput.SwitchCurrentActionMap(controlName);
    }

    public void EnablePlatformerControls()
    {
        EnablePlayerControls(actionMapPlatformerControls);
    }

    public void EnableHelicopterControls()
    {
        EnablePlayerControls(actionMapHelicopterControls);
    }


    public void EnablePauseMenuControls()
    {
        EnablePlayerControls(actionMapMenuControls);
    }
}
