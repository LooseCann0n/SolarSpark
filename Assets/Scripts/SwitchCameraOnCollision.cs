using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SwitchCameraOnCollision : MonoBehaviour
{
    public CinemachineVirtualCamera mainCM;
    public CinemachineVirtualCamera verticalCM;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mainCM.m_Priority = 5;
            verticalCM.m_Priority = 10;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mainCM.m_Priority = 10;
            verticalCM.m_Priority = 5;
        }
    }
}
