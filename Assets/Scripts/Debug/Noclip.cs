using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class Noclip : MonoBehaviour
{
    private bool isEnabled = false;

    public float noClipSpeed;
    public float noClipSprintSpeed;
    private float baseSpeed;

    private PlayerInputActions playerControls;
    private Rigidbody rb;
    private Collider playerCollider;
    public Camera cam;
    public CinemachineVirtualCamera CinemachineVirtualCamera;
    private CinemachinePOV cinemachinePOV;

    private bool noClipSprint;
    private Vector2 moveDirection;
    private Vector3 targetDirection;

    private void OnEnable()
    {
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }


    // Start is called before the first frame update
    void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
        cam = Camera.main;
        cinemachinePOV = CinemachineVirtualCamera.GetCinemachineComponent<CinemachinePOV>();
        baseSpeed = noClipSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerControls.Player.NoClip.triggered)
        {
            Debug.Log("Noclip");
            isEnabled = !isEnabled;
        }

        if (isEnabled == true)
            Fly();
        if (isEnabled == false)
        {
            playerCollider.enabled = true;
            rb.isKinematic = false;
            //cinemachinePOV.m_VerticalAxis.m_MinValue = -12;

            GetComponent<PlayerMovement>().enabled = true;
        }

    }


    private void Fly()
    {
        if (playerControls.Player.NoClipSprint.triggered)
        {
            noClipSprint = !noClipSprint;
        }
        if (noClipSprint == true)
            noClipSpeed = noClipSprintSpeed;
        if (noClipSprint == false)
            noClipSpeed = baseSpeed;

        //cinemachinePOV.m_VerticalAxis.m_MinValue = -70;

        moveDirection = playerControls.Player.Movement.ReadValue<Vector2>();
        ApplyTargetDirection();

        transform.position += targetDirection *  Time.deltaTime * noClipSpeed;

        GetComponent<PlayerMovement>().enabled = false;
        playerCollider.enabled = false;
        rb.isKinematic = true;
    }

    private Vector3 ApplyTargetDirection()
    {
        targetDirection = new Vector3(moveDirection.x, 0, moveDirection.y);
        targetDirection = cam.transform.TransformDirection(targetDirection);

        return targetDirection.normalized;
    }
}
