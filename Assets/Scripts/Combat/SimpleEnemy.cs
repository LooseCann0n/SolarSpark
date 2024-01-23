using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using BehaviourTree;

public class SimpleEnemy : Character, IDamageable
{
    public GameObject AttackIndicator;

    public HealthBar healthBar;
    private CameraLockOn cameraLockOn;
    private CreateAttackLocations attackLocations;
    private Animator animator;
    private NavMeshAgent agent;
    private Component[] limbs;
    private EnemyStance staggerManager;

    [Range(1, 3)] public float stanceBrokenMultipler;
    public float staggerTime;
    public float healthBarScale;
    [HideInInspector]
    public bool playerLocked;

    [Header("References")]
    public Transform lockOnTransform;
    public GameObject topJoint;
    public BoxCollider weaponCollider;
    public GameObject collisionBlocker;

    AudioEnemy enemyAudio;
    Rigidbody rb;

    private void Start()
    {
        cameraLockOn = Camera.main.GetComponent<CameraLockOn>();
        enemyAudio = GetComponent<AudioEnemy>();
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        staggerManager = GetComponent<EnemyStance>();
        limbs = topJoint.GetComponentsInChildren<Rigidbody>();
        rb = GetComponent<Rigidbody>();
        SetKinematic(true);
    }

    public override void Damage(int amount)
    {
        if (staggerManager.stunned == true)
            amount = (int)(amount * stanceBrokenMultipler);

        // REFACTOR TEMP SOLUTION SO IF PLAYER HASN'T BEEN FOUND ENEMY WILL ATTACK
        //if (GetComponent<TheKiwiCoder.BehaviourTreeRunner>().tree.blackboard.playerPosition = null)
        //    GetComponent<TheKiwiCoder.BehaviourTreeRunner>().tree.blackboard.playerPosition = GameObject.FindWithTag("Player").transform;

        base.Damage(amount);
        //if (health > 0)
        //{
        //    int random = Random.Range(0, 2);
        //    animator.SetTrigger("Hit " + random);
        //    StartCoroutine(Stagger());
        //}
    }
    
    void Update()
    {
        if (cameraLockOn.currentLockOnTarget == lockOnTransform)
            playerLocked = true;
        else
            playerLocked = false;

        if (playerLocked == false && health == maxHealth)
        {
            healthBar.gameObject.SetActive(false);
            return;
        }
        if (staggerManager.stunned == true)
            weaponCollider.enabled = false;
        healthBar.gameObject.SetActive(true);
        healthBarScale = (float)Health / (float)maxHealth;
        healthBar.SetSize(healthBarScale);
    }

    public override void Die()
    {       
        if (cameraLockOn.currentLockOnTarget != null)
            cameraLockOn.RemoveLockOn();

        healthBar.SetSize(0f);
        int random = Random.Range(0, 2);
        healthBar.gameObject.SetActive(false);
        enemyAudio.PlayEnemyDeathClip();
        enemyAudio.PlayEnemyDeathChatter();
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponentInChildren<BoxCollider>().enabled = false;
        collisionBlocker.GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;   
        if (GetComponent<TheKiwiCoder.BehaviourTreeRunner>().enabled == true)
            GetComponent<TheKiwiCoder.BehaviourTreeRunner>().enabled = false;
        weaponCollider.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("Corpse");
        SetKinematic(false);
        GetComponent<JackalBt>().enabled = false;
        if (gameObject.name == "Sobek")
        {
            GetComponent<CapsuleCollider>().enabled = true;
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            animator.SetTrigger("Death 0");
            StartCoroutine(GoToCredits());
            healthBar.gameObject.SetActive(false);
        }
        else
        {
            animator.enabled = false;
        }
        //gameObject.AddComponent<Rigidbody>();

        AttackIndicator.SetActive(false);

        GetComponent<SimpleEnemy>().enabled = false;
    }

    private IEnumerator GoToCredits()
    {
        yield return new WaitForSeconds(4);
        UnityEngine.SceneManagement.SceneManager.LoadScene(7);
    }

    private void SetKinematic(bool newValue)
    {
        foreach(Rigidbody rb in limbs)
        {
            rb.isKinematic = newValue;
            if (newValue == false)
                rb.gameObject.layer = LayerMask.NameToLayer("Corpse");
        }
    }

}
