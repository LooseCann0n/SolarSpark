using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    // Update is called once per frame
    void Update()
    {
        HandleMovement();    
    }

    private void HandleMovement()
    {
        // Use main camera transform to make player inputs move player relative to the camera
        Vector3 cameraForward = Camera.main.transform.forward.normalized;
        Vector3 cameraRight = Camera.main.transform.right.normalized;

        if (Input.GetKey(KeyCode.W))
        {
            transform.position += cameraForward * moveSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.position += -cameraRight * moveSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.position += -cameraForward * moveSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.position += cameraRight * moveSpeed * Time.deltaTime;
        }
    }
}
