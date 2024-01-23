using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Animator doorAnim;

    public bool RequiresKey;
    public bool ReqEgyptKey;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (RequiresKey)
            {
                if(ReqEgyptKey && other.GetComponent<PlayerInventory>().hasKey)
                {
                    doorAnim.SetTrigger("OpenDoor");
                }
                
            }
            else
            {
                doorAnim.SetTrigger("OpenDoor");
            }
            
        }
    }

}
