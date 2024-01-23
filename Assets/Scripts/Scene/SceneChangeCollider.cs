using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeCollider : MonoBehaviour
{
    [SerializeField] private int sceneIndex;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //SceneChangeHandler.Instance.LoadLevel(sceneIndex);
        }
    }
}
