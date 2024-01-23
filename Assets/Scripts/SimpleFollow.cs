using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollow : MonoBehaviour
{
    public Transform FollowObject;

    public int Yoffset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = FollowObject.transform.position + new Vector3(0, Yoffset, 0);
    }
}
