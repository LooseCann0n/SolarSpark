using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class EnemySpawning : MonoBehaviour
{
    private PlayerInput playerInput;
    public GameObject enemySpawnUI;
    public GameObject swordEnemy;
    public GameObject axeEnemy;
    public Transform spawnLocation;
    public float overlapRadius;

    public LayerMask enemyLayer;
    public Transform enemyParent;
    private IDisposable m_EventListener;
    private int failedAttempts;
    private int numberOfEnemies;
    // Start is called before the first frame update
    void Start()
    {
        playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
    }

    void Update()
    {
        if (playerInput.actions["SpawnSword"].triggered)
        {
            SpawnEnemy(spawnLocation.position, swordEnemy);
        }
        if (playerInput.actions["SpawnAxe"].triggered)
        {
            SpawnEnemy(spawnLocation.position, axeEnemy);
        }
    }

    private void SpawnEnemy(Vector3 spawnPoint, GameObject enemyToSpawn)
    {
        //if (CheckAreaClear(spawnPoint, overlapRadius) == true)
        Instantiate(enemyToSpawn, spawnPoint, Quaternion.identity, enemyParent); 
        //else
        //{
        //    Vector3 offset = new Vector3(ailedAttempts, 0, 0);
        //    SpawnEnemy(spawnPoint + offset, enemyToSpawn);
        //    failedAttempts++;
        //}
    }

    private bool CheckAreaClear(Vector3 checkArea, float overlapRadius)
    {
        Collider[] hitEnemies = Physics.OverlapSphere(checkArea, overlapRadius, enemyLayer);
        if (hitEnemies.Length > 0)
            return false;

        return true;
    }
}