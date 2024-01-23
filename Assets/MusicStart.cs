using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicStart : MonoBehaviour
{
    [SerializeField] AudioSource source1;
    [SerializeField] AudioSource source2;

    private void OnTriggerEnter(Collider other)
    {
        if (!source1.isPlaying || !source2.isPlaying)
        {
            source1.Play();
            source2.Play();
        }
    }
}
