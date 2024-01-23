using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterPieceRotationController : MonoBehaviour
{
    private Quaternion RandomRotation;
    void Start()
    {
        RandomRotation = Random.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(RandomRotation.eulerAngles * 0.03f * Time.deltaTime);
    }
}
