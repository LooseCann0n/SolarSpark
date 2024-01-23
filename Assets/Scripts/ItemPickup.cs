using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public bool isEgyptKey;
    
    void Start()
    {
        
    }


    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isEgyptKey)
            {
                other.GetComponent<PlayerInventory>().hasKey = true;
                CanvasManager.Instance.UpdateKeys("egypt");
            }
            Destroy(gameObject);
        }
    }
}
