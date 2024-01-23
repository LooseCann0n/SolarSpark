using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Utilities;

public class CursorController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        //InputSystem.onAnyButtonPress.Call(
        //ctrl =>
        //{
        //    if (ctrl.device is Gamepad gamepad)
        //        Cursor.visible = false;
        //    if (ctrl.device is Keyboard keyboard)
        //        Cursor.visible = true;
        //    if (ctrl.device is Mouse mouse)
        //        Cursor.visible = false;
        //    Debug.Log(ctrl.device.displayName);
        //});

        //if (Cursor.visible = false && (Keyboard.current.anyKey.isPressed || Mouse.current.IsPressed()))
        //    Cursor.visible = true;
    }        
}
