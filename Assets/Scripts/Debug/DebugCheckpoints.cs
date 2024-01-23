using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCheckpoints : MonoBehaviour
{
    public Transform[] checkPoints;
    private Transform player;
    public GameObject debugMenu;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void GoCheckpoint(int checkPointNumber)
    {
        player.position = checkPoints[checkPointNumber - 1].position;
    }

    public void BackButton()
    {
        debugMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
