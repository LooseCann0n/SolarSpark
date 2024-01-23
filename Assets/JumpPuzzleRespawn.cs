using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPuzzleRespawn : MonoBehaviour
{

    public GameObject Player;
    public GameObject RespwanPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player.transform.position = RespwanPoint.transform.position;
        }
    }
}
