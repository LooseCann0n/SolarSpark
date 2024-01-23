using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EnemyAttack : MonoBehaviour
{
    private BoxCollider boxCollider;
    [SerializeField]
    private bool blockable;

    [SerializeField]
    private int staggerAmount;

    [SerializeField]
    public int weaponDamage;

    // References
    private Transform player;
    private EnemyStance enemyStance;
    private PlayerStance playerStance;
    private SimpleCombat simpleCombat;
    private PlayerManager playerManager;
    private Animator playerAnimator;
    private bool blocked;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        enemyStance = GetComponentInParent<EnemyStance>();
        player = GameObject.FindWithTag("Player").transform;
        playerStance = player.GetComponent<PlayerStance>();
        simpleCombat = player.GetComponent<SimpleCombat>();
        playerManager = player.GetComponent<PlayerManager>();
        playerAnimator = player.GetComponentInChildren<Animator>();
        GetComponent<Rigidbody>().isKinematic = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        blocked = false;
        if (collision.gameObject.layer == LayerMask.NameToLayer("Block")/* || (collision.gameObject.layer == LayerMask.NameToLayer("Block") && collision.gameObject.layer == LayerMask.NameToLayer("Player"))*/)
        {
            if (!blockable)
            {
                Debug.Log("Kick through block");
                playerStance.Stagger(staggerAmount, 0.4f);
                DealDamage(collision);
                return;
            }
            if (simpleCombat.isParrying) // Enemy has hit player whilst in a parry time window
            {
                boxCollider.enabled = false; // Turn off enemy collider
                enemyStance.Stagger(100, 2f); // Stagger enemy for max
                playerManager.HitParried();
                blocked = true;
            }
            else // Player has been hit in a standard block
            {
                playerAnimator.SetTrigger("blockTrigger");
                if (playerStance.Stance + staggerAmount >= playerStance.stanceBreakpoint)
                {
                    blocked = false;
                }
                else
                {
                    blocked = true;
                    boxCollider.enabled = false;
                }
                playerStance.Stagger(staggerAmount, 0.3f); // Stagger player 

                return;
            }
        }
        if (blocked == false)
        {
            DealDamage(collision);
        }
    }

    private void DealDamage(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (collision.gameObject.GetComponent<PlayerManager>().Health > 0)
            {
                // If collided with a damageable object do damage
                IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
                // Object has no damageable component, do exit spaghetti code
                if (damageable == null)
                    return;
                damageable.Damage(weaponDamage);
                boxCollider.enabled = false;
            }
        }
    }
}
