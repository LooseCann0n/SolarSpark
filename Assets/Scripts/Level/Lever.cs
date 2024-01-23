using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Lever : MonoBehaviour
{
    public GameObject objectToDestoy;
    public float cameraSwitchTime;
    //public Animation button;

    public bool buttonPressed;
    public AudioClip buttonClip;
    private CinemachineSwitchCamera switchCamera;
    [SerializeField]
    private CinemachineVirtualCamera vcam;

    private void Start()
    {
        switchCamera = GetComponent<CinemachineSwitchCamera>();
        //tempViewCam.enabled = false;
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && buttonPressed == false)
        {
            Destroy(objectToDestoy);
            if(GetComponent<AudioSource>() != null)
            {
                GetComponent<AudioSource>().PlayOneShot(buttonClip);
            }
            
            StartCoroutine(SwitchActiveCam());
            //if (!openScript.leverOnePressed && !openScript.leverTwoPressed)
            //    StartCoroutine(openScript.ActivateTempCam(mainCam, tempViewCam, false));
            buttonPressed = true;
        }
    }

    private IEnumerator SwitchActiveCam()
    {
        MasterCameraController.Instance.ChangeCamEnabled(vcam, true, true);
        yield return new WaitForSeconds(cameraSwitchTime);
        MasterCameraController.Instance.ChangeCamEnabled(vcam, false, false);
    }
}
