using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipTrigger : MonoBehaviour
{
    [SerializeField]
    private DialoguePair lineToPlay;

    private bool clipPlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!clipPlayed)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                PlayerVoice playerSource = other.gameObject.GetComponent<PlayerVoice>();
                playerSource.PlayClip(lineToPlay);
                clipPlayed = true;
            }
        }
    }
}
