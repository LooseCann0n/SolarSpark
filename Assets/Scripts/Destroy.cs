using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlaySound());
    }

    IEnumerator PlaySound()
    {
        yield return new WaitForSeconds(2);

        Destroy(gameObject);   
    }
}
