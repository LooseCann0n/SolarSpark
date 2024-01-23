using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using Cinemachine;

public class ResetPlayer : MonoBehaviour
{
    private CinemachineVirtualCamera vcam;
    private CinemachinePOV pov;
    private Transform player;
    [SerializeField]
    private Transform respawnPoint;
    public float distanceFromPlayer;
    public float minimumResetPosition;
    public List<Transform> checkPoints = new List<Transform>();

    private void Start()
    {
        vcam = GameObject.Find("Main CM Vcam").GetComponent<CinemachineVirtualCamera>();
        player = GameObject.FindWithTag("Player").transform;
        pov = vcam.GetCinemachineComponent<CinemachinePOV>();
    }


    private void Update()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y - distanceFromPlayer, player.transform.position.z);
        if (transform.position.y < minimumResetPosition)
        {
            transform.position = new Vector3(player.transform.position.x, minimumResetPosition, player.transform.position.z);
        }
    }

    public Transform FindClosestCheckpoint()
    {
        float shortestDistance = Mathf.Infinity;

        for (int i = 0; i < checkPoints.Count; i++)
        {
            float distanceToCheckpoint = Vector3.Distance(checkPoints[i].position, player.transform.position);
            if (distanceToCheckpoint < shortestDistance)
            {
                // Assign shortestDistance as distanceFromPlayer
                shortestDistance = distanceToCheckpoint;
                respawnPoint = checkPoints[i];                
            }
        }
        return respawnPoint;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            respawnPoint = FindClosestCheckpoint();
            player.position = respawnPoint.position;
            player.rotation = respawnPoint.rotation;
            pov.m_HorizontalAxis.Value = 180;
            pov.m_VerticalAxis.Value = 0;
        }
    }
}
