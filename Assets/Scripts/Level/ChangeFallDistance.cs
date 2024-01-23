using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFallDistance : MonoBehaviour
{
    public ResetPlayer resetPlayer;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            resetPlayer.minimumResetPosition = -1000;
        }
    }
}
