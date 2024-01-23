using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MasterCameraController : MonoBehaviour
{
    public static MasterCameraController Instance { get; private set; }

    [SerializeField] private CinemachineVirtualCamera mainCam;

    private int maxPriority = 0;

    private void Awake()
    {
        Instance = this;
    }
    /// <summary>
    /// Switches the camera temporarily to a given camera
    /// </summary>
    /// <param name="vcam">The camera to switch to</param>
    /// <param name="switchDuration">How long to switch for</param>
    /// <param name="shouldOverride">Whether to override any current camera being prioritised</param>
    /// <param name="disableMovement">Whether to disable movement whilst camera is switching</param>
    /// <returns>A wait in time before switching the camera priority to 0</returns>
    public IEnumerator TempSwitchCam(CinemachineVirtualCamera vcam, float switchDuration, bool shouldOverride, bool disableMovement)
    {
        int priority = 99;
        if (shouldOverride && maxPriority != 0)
        {
            priority = maxPriority + 1;
        }

        vcam.m_Priority = priority;
        if (disableMovement)
        {
            InteractHandler.Instance.canMove = false;
            InteractHandler.Instance.canInteract = false;
            PlayerMovement.Instance.canMove = false;
        }


        yield return new WaitForSeconds(switchDuration);

        vcam.m_Priority = 0;
        if (disableMovement)
        {
            InteractHandler.Instance.canMove = true;
            InteractHandler.Instance.canInteract = true;
            PlayerMovement.Instance.canMove = true;
        }
    }

    /// <summary>
    /// Makes a camera the main priority
    /// </summary>
    /// <param name="vcam"></param>
    /// <param name="enable"></param>
    public void ChangeCamEnabled(CinemachineVirtualCamera vcam, bool enable, bool disableMovement)
    {
        PlayerMovement.Instance.canMove = !disableMovement;

        if (enable)
        {
            vcam.m_Priority = 99;
        }
        else
        {
            vcam.m_Priority = 0;
        }
    }
}
