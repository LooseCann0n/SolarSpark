using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private PlayerInputActions playerControls;
    private CinemachineVirtualCamera vCam;
    private CinemachinePOV cinemachinePOV;
    private Transform player;
    public float transitionTime;
    private float timer;
    private bool resetCamera;
    public bool noTargets;

    private void Awake()
    {
        playerControls = new PlayerInputActions();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        vCam = GameObject.Find("Main CM Vcam").GetComponent<CinemachineVirtualCamera>();
        cinemachinePOV = vCam.GetCinemachineComponent<CinemachinePOV>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }

    //public void Update()
    //{
    //    if (resetCamera == true)
    //        transitionTime += Time.deltaTime;

    //    if (transitionTime >= (cinemachinePOV.m_HorizontalRecentering.m_RecenteringTime * 4 + cinemachinePOV.m_HorizontalRecentering.m_WaitTime))
    //    {
    //        transitionTime = 0;
    //        cinemachinePOV.m_HorizontalRecentering.m_enabled = false;
    //        cinemachinePOV.m_VerticalRecentering.m_enabled = false;
    //        resetCamera = false;
    //    }

    //    if (playerControls.Player.ResetCamera.triggered && noTargets)
    //    {
    //        Debug.Log("Reset camera");
    //        ResetCamera();           
    //    }

    //}

    //public void ResetCamera()
    //{
    //    cinemachinePOV.m_HorizontalRecentering.m_enabled = true;
    //    cinemachinePOV.m_VerticalRecentering.m_enabled = true;
    //    resetCamera = true;
    //}

}