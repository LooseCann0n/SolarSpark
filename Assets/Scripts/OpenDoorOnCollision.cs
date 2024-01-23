using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class OpenDoorOnCollision : MonoBehaviour
{
    public GameObject Door;
    [SerializeField]
    private CinemachineVirtualCamera virtualCam;
    [SerializeField]
    private float cameraSwitchTime;

    public bool buttonpressed = false;

    AudioSource targetSource;
    public AudioClip pressClip;

    private void Start()
    {
        targetSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Door.SetActive(false);
            if(targetSource != null)
                targetSource.PlayOneShot(pressClip);
            buttonpressed = true;
            StartCoroutine(MasterCameraController.Instance.TempSwitchCam(virtualCam, cameraSwitchTime, true, true));
        }

    }
}
