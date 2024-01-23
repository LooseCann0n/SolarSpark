using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableVase : MonoBehaviour
{
    public GameObject AudioSource;
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Khopesh"))
        {
            Destroy(gameObject);
            Instantiate (AudioSource, transform.position, transform.rotation);
        }
    }
}
