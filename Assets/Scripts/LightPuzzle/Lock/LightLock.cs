using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LightLock : MonoBehaviour
{
    [HideInInspector] public bool unlocked;

    [Header("Misc")]
    [Tooltip("The object to enable when unlocked"), SerializeField] private GameObject flame;
    [Tooltip("The master lock that this lock is linked to"), SerializeField] private MasterLock myLock;
    public AudioClip successClip;
    public AudioSource targetSource;

    [Header("Camera")]
    [Tooltip("The cam to switch to when unlocked"), SerializeField] private CinemachineVirtualCamera virtualCam;
    [Tooltip("The amount of time to stay on the temporary cam"), SerializeField] private float cameraSwitchTime;

    private void Awake()
    {
        unlocked = false;
    }

    /// <summary>
    /// Unlocks or locks depending on the given boolean value
    /// </summary>
    /// <param name="unlock">whether the lock is locked or not</param>
    public void Unlock(bool unlock)
    {
        // if already in correct state, return as no action needed
        if(unlocked == unlock)
        {
            return;
        }

        // enable the flame depdnign on unlock value
        flame.SetActive(unlock);
        
        // if true, switch the cam to show it has bee ncompleted
        if(unlock == true)
        {
            InteractHandler.Instance.RemoveFromInteractObject(true);
            StartCoroutine(MasterCameraController.Instance.TempSwitchCam(virtualCam, cameraSwitchTime, true, true));
            PlaySuccess();
        }
        
        // set unlocked value and check lock to see if it can unlock
        unlocked = unlock;
        myLock.CheckUnlocked();
    }

    public void PlaySuccess()
    {
        if(targetSource != null)
        {
            targetSource.PlayOneShot(successClip);
        }
    }
}
