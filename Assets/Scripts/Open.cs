using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Open : MonoBehaviour
{ 
    public GameObject Rock;
    public bool buttonpressed;
    public float viewTime;
    public Cinemachine.CinemachineVirtualCamera tempCam;
    public CinemachineSwitchCamera cinemachineSwitchCamera;

    private void Start()
    {
        buttonpressed = false;
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player" && buttonpressed == false)
        {
            Rock.SetActive(false);
            buttonpressed = true;
            StartCoroutine(cinemachineSwitchCamera.SwitchState(tempCam, viewTime));
        }
    }
}
