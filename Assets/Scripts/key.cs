using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class key : MonoBehaviour
{
    public GameObject Key;
    public GameObject Door;
    public GameObject NoKeyUI;
    private bool HasKey;
   
    // Start is called before the first frame update
    void Start()
    {
        HasKey = false;
        NoKeyUI.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == Key)
        {
            Key.SetActive(false);
            HasKey = true;
        }
        else  if (collision.gameObject == Door)
        {
            if (HasKey == true)
            {
                Door.SetActive(false);
            }
            else
            {
                StartCoroutine(NoKey());
            }
        }

    }
    IEnumerator NoKey()
    {
        NoKeyUI.SetActive(true);
        yield return new WaitForSeconds (3);
        NoKeyUI.SetActive(false);
    }

}
