using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LeverOpen : MonoBehaviour
{
    public GameObject leverOne;
    public GameObject leverTwo;
    private CinemachineSwitchCamera switchCamera;
    public CinemachineVirtualCamera vcam;

    [HideInInspector]
    public bool leverOnePressed;
    [HideInInspector]
    public bool leverTwoPressed;

    public bool openDoor;

    [SerializeField] private float camSwitchDelay;

    private void Start()
    {
        switchCamera = GetComponent<CinemachineSwitchCamera>();
        //cam2.GetComponent<Camera>().enabled = false;
    }

    private void Update()
    {
        leverOnePressed = leverOne.GetComponent<Lever>().buttonPressed;
        leverTwoPressed = leverTwo.GetComponent<Lever>().buttonPressed;

        if (leverOnePressed && leverTwoPressed && openDoor == false)
        {
            openDoor = true;
            StartCoroutine(switchCamera.SwitchState(vcam, camSwitchDelay, 15));
            //StartCoroutine(ActivateTempCam(cam1, cam2.GetComponent<Camera>(), true));
        }
    }

    //public IEnumerator ActivateTempCam(GameObject mainCam, Camera tempViewCam, bool destroyObj)
    //{
    //    if(tempViewCam == null || mainCam == null)
    //    {
    //        yield return null;
    //    }

    //    yield return new WaitForSeconds(1.5f);

    //    mainCam.SetActive(false);
    //    tempViewCam.enabled = true;
    //    yield return new WaitForSeconds(camSwitchDelay);
    //    mainCam.SetActive(true);
    //    tempViewCam.enabled = false;

    //    if (destroyObj)
    //    {
    //        this.enabled = false;
    //    }
    //}

}
