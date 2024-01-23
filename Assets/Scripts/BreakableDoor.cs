using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableDoor : MonoBehaviour //IDamageable
{
    public AudioSource audioSource;
    public GameObject AudioSource;
    public GameObject DoorBreakText;
    [SerializeField] protected float health;
    [SerializeField] protected int maxHealth;

    public float Health
    {
        get => health;
        set => health = value;
    }


    private void Start()
    {
        DoorBreakText.SetActive(false);
    }

    // private void OnTriggerEnter(Collision collision)

    // if (collision.gameObject.CompareTag("SpecialAttack"))

    // Destroy(gameObject);
    // Instantiate(AudioSource, transform.position, transform.rotation);

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SpecialAttack"))
        {
            Die();
        }
    }

    private void OnDestroy()
    {
        Destroy(DoorBreakText);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(OpenText());
        }
    }

    IEnumerator OpenText()
    {
        DoorBreakText.SetActive(true);
        yield return new WaitForSeconds(3);
        DoorBreakText.SetActive(false);
    }

   // public void Damage(int damageAmount)
  //  {
        // 20 = heavy atack damage
       // if(damageAmount == 20)
     //   {
            //Die();
      //  }
        /*
        Health -= damageAmount;
        audioSource.Play();
        Debug.Log("Reeive dmg");

        if (health <= 0)
            Die();
        */
  //  }



    protected virtual void Awake()
    {
        health = maxHealth;
    }

    public virtual void Die()
    {
        Instantiate(AudioSource, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}