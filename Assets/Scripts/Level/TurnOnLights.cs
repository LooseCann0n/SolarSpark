using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnLights : MonoBehaviour
{
    [SerializeField] private GameObject[] torchFire;
    [SerializeField] private GameObject[] bigSmoke;
    private bool triggered;

    private void Awake()
    {
        for (int i = 0; i < torchFire.Length; i++)
        {
            torchFire[i].SetActive(false);
            torchFire[i].transform.localScale = new Vector3(torchFire[i].transform.localScale.x, 1, torchFire[i].transform.localScale.z);
            bigSmoke[i].SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && triggered == false)
        {
            PlayParticles();
        }
    }

    private void PlayParticles()
    {
        for (int i = 0; i < torchFire.Length; i++)
        {
            torchFire[i].SetActive(true);
            torchFire[i].GetComponent<Animator>().Play("TorchStart");
            bigSmoke[i].SetActive(true);
        }
        triggered = true;
    }
}
