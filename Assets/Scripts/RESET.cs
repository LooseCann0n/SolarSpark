using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RESET : MonoBehaviour
{
    public Transform player;
    public Transform respawnpoint;


    // Start is called before the first frame update
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.position = respawnpoint.position;
        }
    }
}
