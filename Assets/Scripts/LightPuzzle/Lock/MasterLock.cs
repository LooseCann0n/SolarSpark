using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MasterLock : MonoBehaviour
{
    [Tooltip("The light locks that fall under this master lock"), SerializeField] LightLock[] lightlocks;

    private Animator animator;
    [SerializeField]
    private bool isOpen;
    [SerializeField] private string openLockTrigger;
    [SerializeField] private string closeLockTrigger;

    public CinemachineVirtualCamera virtualCam;
    public float cameraSwitchTime;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        isOpen = false;
    }

    /// <summary>
    /// Checks whether to unlock the door or not based on this subsequent locks
    /// </summary>
    public void CheckUnlocked()
    {
        // check through all lightlocks
        for (int i = 0; i < lightlocks.Length; i++)
        {
            // if any lock is locked, ensure the door is closed and return
            if (!lightlocks[i].unlocked)
            {
                if (isOpen)
                {
                    animator.SetTrigger(closeLockTrigger);
                }
                return;
            }
        }
        StartCoroutine(MasterCameraController.Instance.TempSwitchCam(virtualCam, cameraSwitchTime, true, true));

        animator.SetTrigger(openLockTrigger);
        isOpen = true;
        InteractHandler.Instance.RemoveFromInteractObject(true);
    }
}
