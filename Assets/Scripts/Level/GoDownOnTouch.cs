using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoDownOnTouch : MonoBehaviour
{
    public GameObject enemy;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GetComponent<Animation>().Play();
            enemy.SetActive(true);
        }
    }
}
