using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RotateOnTouch : MonoBehaviour
{
    [Tooltip("The speed to rotate at")] public float rotationSpeed;
    [Tooltip("The part the player pushes and interacts with to rotate the object")] public GameObject pushSection;

    [Tooltip("The max angle the player can be from the push forward direction"), SerializeField] private float maxPushAngle;

    public CinemachineVirtualCamera myCam;

    private PlayerInputActions playerControls;
    private int directionModifier;

    private Transform playerTransform;

    private void Awake()
    {
        playerControls = new PlayerInputActions();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Update()
    {
        if(InteractHandler.Instance.isInteracting && InteractHandler.Instance.canMove && InteractHandler.Instance.currentRotator == this)
        {
            // get input direction
            Vector2 dir = playerControls.Player.Movement.ReadValue<Vector2>();

            // rotate the object
            if (dir.x != 0)
            {
                transform.Rotate(transform.up * (rotationSpeed) * dir.x * Time.deltaTime);
            }
        }
    }

    /// <summary>
    /// Sets the direction modifier depending on the direction the player is pushing from
    /// </summary>
    public void SetDirection()
    {
        float angle = Vector3.Angle(pushSection.transform.forward, playerTransform.forward);

        if (angle < maxPushAngle)
        {
            directionModifier = 1;
        }
        else
        {
            directionModifier = -1;
        }
    }
}
