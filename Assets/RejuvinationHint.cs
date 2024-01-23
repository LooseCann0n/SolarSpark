using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RejuvinationHint : MonoBehaviour
{
    public GameObject HintUI;
    // Start is called before the first frame update
    void Start()
    {
        HintUI.SetActive(false); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("enter");
            StartCoroutine(OpenText());
        }
    }
    IEnumerator OpenText()
    {
        HintUI.SetActive(true);
        yield return new WaitForSeconds(3);
        HintUI.SetActive(false);
        Destroy(this);
    }
}
