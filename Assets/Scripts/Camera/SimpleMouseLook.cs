using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMouseLook : MonoBehaviour
{
    //[SerializeField] private float sensitivity;
    //[SerializeField] Transform player;

    //private float xRot = 0f;
   
    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    //void Update()
    //{
    //    float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
    //    float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

    //    xRot -= mouseY;
    //    xRot = Mathf.Clamp(xRot, -90, 90);

    //    transform.localRotation = Quaternion.Euler(xRot, player.transform.rotation.y, player.transform.rotation.z);
    //    player.Rotate(Vector3.up * mouseX);
    //}
}
