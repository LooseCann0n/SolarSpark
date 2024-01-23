using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, IDamageable
{

    [SerializeField] protected float health;
    [SerializeField] protected int maxHealth;

    // The amount of health
    public float Health
    {
        get => health;
        set => health = value;
    }

    /// <summary>
    /// Apply an amount of damage
    /// </summary>
    /// <param name="amount"> The amount of damage to apply </param>
    public virtual void Damage(int amount)
    {
        Health -= amount;

        if (health <= 0)
            Die();
    }

    protected virtual void Awake()
    {
        health = maxHealth;
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }
}
