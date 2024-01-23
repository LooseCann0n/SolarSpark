using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class AddCheckpoint : MonoBehaviour
{
    private ResetPlayer resetPlayer;
    private bool hasEntered;

    private void Awake()
    {
        resetPlayer = GameObject.Find("PlayerReset").GetComponent<ResetPlayer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasEntered)
        {
            Debug.Log("Player entered");
            resetPlayer.checkPoints.Add(transform);
            hasEntered = true;
        }
    }
}
