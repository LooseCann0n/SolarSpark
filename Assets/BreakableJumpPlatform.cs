using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableJumpPlatform : MonoBehaviour
{
    public GameObject BreakablePlatform;
    public GameObject AudioSource;

    public int breakDelay;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine (Break());
        }
    }
    IEnumerator Break()
    {
        yield return new WaitForSeconds(breakDelay);

        Destroy(BreakablePlatform);

        Instantiate(AudioSource, transform.position, transform.rotation);
    }
}
