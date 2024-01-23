using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObjects : MonoBehaviour
{
    public GameObject AudioSource;
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Khopesh"))
        {
            OnHit();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SpecialAttack"))
        {
            OnHit();
        }
    }

    private void OnHit()
    {
        Destroy(gameObject);
        Instantiate(AudioSource, transform.position, transform.rotation);
    }
}
