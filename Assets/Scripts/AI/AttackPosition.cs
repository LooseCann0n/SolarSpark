using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPosition : MonoBehaviour
{
    public Transform enemyUsing;
    private SphereCollider sphereCollider;
    public bool areaOccupied;

    private void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy enters");
            enemyUsing = other.transform;
            areaOccupied = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == enemyUsing)
        {
            Debug.Log("Enemy leaves");
            enemyUsing = null;
            areaOccupied = false;
        }
    }
}
