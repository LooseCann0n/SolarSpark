using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public List<BasicEnemy> enemyList = new List<BasicEnemy>();

    public List<AttackPosition> attackingLocations = new List<AttackPosition>();
    public CreateAttackLocations attackSpots;

    private void Start()
    {
        attackingLocations = attackSpots.attackPositions;
    }

    private void Update()
    {
        //if (attackingLocations.Count < 0)
        //{
        //    return;
        //}

        //for (int i = 0; i < attackingLocations.Count; i++)
        //{
        //    FindClosestEnemy();
        //}

    }

    //private void Update()
    //{
    //    // Only find closest enemy has more than 0 items
    //    if (enemyList.Count < 0)
    //        return;
    //    FindClosestEnemy();
    //    if (enemyList.Count == 1)
    //    {
    //        currentAttacker = enemyList[0];
    //        currentAttacker.attackEnemy = true;

    //    }
    //}

    /// <summary>
    /// Go through enemy list and find the closest enemy, assign that enemy as the current attacker
    /// </summary>
    private void FindClosestEnemy()
    {
        // Float to store the current closest enemy to the player
        float shortestDistance = Mathf.Infinity;

        // For loop to iterate through list
        for (int i = 0; i < enemyList.Count - 1; i++)
        {
            // If list is null (from being destroyed) remove from list
            if (enemyList[i] == null)
                enemyList.Remove(enemyList[i]);

            // Extra check to return as i is still being used to run through loop
            //if (enemyList[i] == null)
            //    return;
            // Calculate the distance from the player to the enemy
            float distanceFromPlayer = Vector3.Distance(transform.position, enemyList[i].transform.position);

            // If the distance is less than the current shortest distance
            if (distanceFromPlayer < shortestDistance)
            {
                // Assign shortestDistance as distanceFromPlayer as that is now the shortest distance from the player
                shortestDistance = distanceFromPlayer; 
            }

        }        
    }
}
