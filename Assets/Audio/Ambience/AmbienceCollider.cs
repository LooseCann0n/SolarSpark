using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceCollider : MonoBehaviour
{
    public AmbienceControl control;
    public bool lowPassActive = false;
    // Start is called before the first frame update
    void Start()
    {
        control = GameObject.Find("Ambience").GetComponent<AmbienceControl>();
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (!lowPassActive)
            {
                control.LowPassOn();
                lowPassActive = true;
            }
            else
            {
                control.LowPassOff();
                lowPassActive=false;
            }
        }
    }
}
