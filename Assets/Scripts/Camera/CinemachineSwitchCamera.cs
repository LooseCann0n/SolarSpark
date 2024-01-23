using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineSwitchCamera : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera mainCamera;

    public IEnumerator SwitchState(CinemachineVirtualCamera vcam, float switchDuration)
    {
        vcam.m_Priority = 10;
        mainCamera.m_Priority = 5;
        yield return new WaitForSeconds(switchDuration);
        vcam.m_Priority = 5;
        mainCamera.m_Priority = 10;
    }

    public IEnumerator SwitchState(CinemachineVirtualCamera vcam, float switchDuration, int vcamPriority)
    {
        vcam.m_Priority = vcamPriority;
        mainCamera.m_Priority = 5;
        yield return new WaitForSeconds(switchDuration);
        vcam.m_Priority = 5;
        mainCamera.m_Priority = 10;
    }
}

