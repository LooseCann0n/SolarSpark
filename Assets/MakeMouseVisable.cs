using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeMouseVisable : MonoBehaviour
{

    void Update()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
