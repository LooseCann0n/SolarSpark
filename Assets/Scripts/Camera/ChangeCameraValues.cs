using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.InputSystem.Controls;
using TMPro;
public class ChangeCameraValues : MonoBehaviour
{
    #region camSensitivity
    [SerializeField]
    public Slider camSensitivitySlider;
    private double camValue;
    public TextMeshProUGUI sensitivityValue;

    #endregion

    #region fieldOfView
    [SerializeField]
    public Slider fovSlider;
    private double distanceValue;
    public TextMeshProUGUI fovValueText;

    #endregion

    #region camDistance
    [SerializeField]
    public Slider camDistanceSlider;
    public TextMeshProUGUI camDistanceText;
    #endregion

    #region camReferences
    private CinemachineVirtualCamera virtualCamera;
    private CinemachinePOV cinemachinePOV;
    private CinemachineFramingTransposer framingTransposer;
    #endregion

    private bool initalised;
    private PlayerInputActions playerControls;

    private void Awake()
    {
        virtualCamera = GameObject.Find("Main CM Vcam").GetComponent<CinemachineVirtualCamera>();
        cinemachinePOV = virtualCamera.GetCinemachineComponent<CinemachinePOV>();
        framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        playerControls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }

    void Start()
    {
        camSensitivitySlider.minValue = 0.05f;
        camSensitivitySlider.maxValue = 0.20f;
        camSensitivitySlider.value = 0.15f;

        fovSlider.maxValue = 90;      
        fovSlider.minValue = 60;
        fovSlider.value = 70;
        
        camDistanceSlider.maxValue = 8f;
        camDistanceSlider.minValue = 4f;
        camDistanceSlider.value = 6f;

        initalised = true;
    }

    public void CameraSensitivity()
    {
        camValue = camSensitivitySlider.value * 5;
        camValue = System.Math.Round(camValue, 2);
        sensitivityValue.text = camValue.ToString();
        cinemachinePOV.m_VerticalAxis.m_MaxSpeed = camSensitivitySlider.value / 2;
        cinemachinePOV.m_HorizontalAxis.m_MaxSpeed = camSensitivitySlider.value;
    }


    public void FOVValue()
    {
        int displayValue = (int)fovSlider.value;
        fovValueText.text = displayValue.ToString();
        virtualCamera.m_Lens.FieldOfView = fovSlider.value;
    }

    public void CameraDistance()
    {
        int displayValue = (int)camDistanceSlider.value;
        camDistanceText.text = displayValue.ToString();
        framingTransposer.m_CameraDistance = camDistanceSlider.value;
    }
}
