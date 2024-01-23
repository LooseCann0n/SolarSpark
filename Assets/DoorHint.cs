using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHint : MonoBehaviour
{
    public GameObject hintText;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(OpenText());
        }
    }

    IEnumerator OpenText()
    {
        hintText.SetActive(true);
        yield return new WaitForSeconds(3);
        hintText.SetActive(false);
    }

    private void OnDisable()
    {
        hintText.SetActive(false);
    }
}
