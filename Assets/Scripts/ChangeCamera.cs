using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class ChangeCamera : MonoBehaviour
{
    public CinemachineVirtualCamera mainCM;
    public CinemachineVirtualCamera verticalCM;

    public bool entering;

    private void ChangeCameraVertical(Collider player)
    {
        if (player.CompareTag("Player"))
        {
            mainCM.m_Priority = 5;
            verticalCM.m_Priority = 10;
        }
    }

    private void ResetCamera(Collider player)
    {
        if (player.CompareTag("Player"))
        {
            mainCM.m_Priority = 10;
            verticalCM.m_Priority = 5;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (entering)
            ChangeCameraVertical(other);
        else
            ResetCamera(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (entering)
            ResetCamera(other);
        else
            ChangeCameraVertical(other);
    }

}
