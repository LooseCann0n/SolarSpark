//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.AI;

//public class EnemyAI : MonoBehaviour
//{
//    [Header("References")]
//    public Transform lockOnTransform;
//    public NavMeshAgent agent;
//    public Transform player;
//    public GameObject weapon;
//    private Animator animator;
//    private AttackManager attackManager;
//    private Material weaponMaterial;
//    private SimpleEnemy simpleEnemy;

//    [Header("AI Variables")]
//    public float walkRadius;
//    public float detectionRange;
//    public float attackRange;
//    public float overlapRadius;
//    public float attackSpeed;
//    public float waitRange;
//    public int weaponDamage;
//    public Color weaponFlashColour;

//    [Header("Behaviour Bools")]
//    public bool canAttack = true;
//    public bool foundPlayer;
//    public bool inRange;
//    public bool attackEnemy;

//    /// <summary>
//    /// Get references to various components
//    /// </summary>
//    private void Start()
//    {
//        animator = GetComponent<Animator>();
//        attackManager = player.GetComponent<AttackManager>();
//        weaponMaterial = weapon.GetComponent<MeshRenderer>().material;
//    }

//    /// <summary>
//    /// Main AI behaviour
//    /// </summary>
//    private void Update()
//    {

//        // If player hasn't been found
//        if (foundPlayer == false)
//            NoDetection(); // Run NoDetection

//        // Player has been detected
//        if (foundPlayer == true)
//        {

//            if (attackEnemy == true)
//                ChasePlayer();
//            if (attackEnemy == false)
//                ChasePlayer();

//            // If the player is in attackRange
//            if (PlayerToEnemyDistance(attackRange))
//            {
//                Debug.Log("In attack range");
//                agent.isStopped = true; // Stop the agent
//                if (canAttack == true) 
//                    animator.SetBool("Attack", true); // Play attack since canAttack is true
//                if (canAttack == false)
//                    animator.SetBool("Attack", false); // Stop playing attack
//                inRange = true; // InRange set to true
//            }
//            if (!PlayerToEnemyDistance(attackRange)) // If the player is out of attack range
//            {
//                animator.SetBool("Attack", false); // Stop attacking
//                inRange = false; // InRange bool set to false
//                agent.isStopped = false; // Allow for movement again
//            }
//            if (!PlayerToEnemyDistance(detectionRange)) // If the player is out of detectionRange
//            {
//                foundPlayer = false; // Found player set to false
//                attackManager.enemyList.Remove(this); // Remove player from enemyList
//            }
//        }

//    }
    
//    /// <summary>
//    /// AI will move to a random position and if the player is in detection range will hunt
//    /// </summary>
//    private void NoDetection()
//    {
//        animator.SetBool("Attack", false); // Reset attack to stop spamming attack to players who are out of detection range
//        if (PlayerToEnemyDistance(detectionRange)) // If player is in detection Range
//        {
//            foundPlayer = true; // Found player set to true
//            attackManager.enemyList.Add(this); // Add this enemy to enemyList
//        }
//        else // If enemy outside detection range
//        {
//            Vector3 randomPosition = PickRandomLocation(); // Pick random location around enemy

//            if (agent.remainingDistance <= agent.stoppingDistance) // If the remainingDistance is less than the stopping 
//            {
//                agent.SetDestination(randomPosition); // Move to the randomly selected location
//            }
//        }
//    }

//    /// <summary>
//    /// Find distance between player and enemy, if it is passed float value then return true, otherwise return false
//    /// </summary>
//    /// <param name="detectionRange"> The range for the distance calucation to be greater than</param>
//    /// <returns></returns>
//    private bool PlayerToEnemyDistance(float detectionRange)
//    {
//        // Distance calculation between player and the enemy
//        float distanceBetween = Vector3.Distance(player.position, transform.position);

//        if (distanceBetween <= detectionRange) // If distance is less than detectionRange then
//        {
//            return true;
//        }
//        return false;
//    }
   
//    /// <summary>
//    /// SetDestination to player position if currentAttacker is true, else, move to player but wait out of range
//    /// </summary>
//    private void ChasePlayer()
//    {
//        if (attackEnemy) // CurrentAttacker is true
//            agent.SetDestination(player.position); // Set agent destination directly on player
//        if (!attackEnemy) // CurrentAttacker is false
//        {
//            // Set agent destination to player position but - waitRange 
//            agent.SetDestination(player.position - transform.forward * waitRange);

//            if (agent.remainingDistance <= waitRange - 1) // If within 1 unit of destination
//            {
//                agent.isStopped = true; // Stop
//            }
//            else // Not at destination
//            {
//                agent.isStopped = false; // Allow movement
//            }
//        }
//        //CheckDestinationClear(agent.destination);


//        //Vector3 dir = player.position - transform.position;
//        //dir.y = 0;//This allows the object to only rotate on its y axis
//        //Quaternion rot = Quaternion.LookRotation(dir);
//        //transform.rotation = Quaternion.Lerp(transform.rotation, rot, rotationSpeed * Time.deltaTime);
//    }

//    //private void CheckDestinationClear(Vector3 destination)
//    //{
//    //    Collider[] colliders = Physics.SphereCast(destination, overlapRadius, transform.forward, out hit)
//    //}

//    /// <summary>
//    /// Pick a random location around the gameObject
//    /// </summary>
//    /// <returns> Vector3 position </returns>
//    private Vector3 PickRandomLocation()
//    {
//        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;

//        randomDirection += transform.position;
//        NavMeshHit hit;
//        NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
//        Vector3 finalPosition = hit.position;
//        return finalPosition;
//    }

//    /// <summary>
//    /// Change weapon colour for duration of coroutine
//    /// </summary>
//    public IEnumerator FlashWeapon()
//    {
//        weaponMaterial.SetColor("_EmissionColor", weaponFlashColour);
//        weaponMaterial.EnableKeyword("_EMISSION");
//        yield return new WaitForSeconds(0.15f);
//        weaponMaterial.SetColor("_EmissionColor", Color.white);
//        weaponMaterial.DisableKeyword("_EMISSION");
//    }

//    public IEnumerator AttackCooldown()
//    {
//        canAttack = false;
//        yield return new WaitForSeconds(attackSpeed);
//        canAttack = true;
//    }
//}
