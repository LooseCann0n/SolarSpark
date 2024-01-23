using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCollision : MonoBehaviour
{

    [SerializeField]
    SimpleCombat combat;
    public ParticleSystem impactSparks;
    [SerializeField]
    float sparkDuration;
    private List<GameObject> enemiesHit;

    private void Start()
    {
        enemiesHit = new List<GameObject>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") && !enemiesHit.Contains(collision.gameObject))
        {
            // If collided with a damageable object do damage
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
            // Object has no damageable component, do exit
            if (damageable == null)
                return;           
            damageable.Damage(combat.playerDamage);
            if (!collision.gameObject.CompareTag("Enemy"))
                return;
            FindHitDirection(collision);
            Actions.OnEnemyHit?.Invoke();
            StartCoroutine(SparkImpact(sparkDuration));
            enemiesHit.Add(collision.gameObject);
            if (damageable.Health > 0)
            {
                IStaggerable staggerable = collision.gameObject.GetComponent<IStaggerable>();
                staggerable.Stagger(combat.staggerDamage, 0f);
                collision.gameObject.GetComponent<AudioEnemy>().PlayEnemyDamageClip();
            }
            combat.EnemyHit();
            if (combat.lightAttack)
                combat.DisableHitbox();
            
        }
    }

    public void FindHitDirection(Collision collision)
    {
        ContactPoint contact = collision.GetContact(0);
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point;

    }

    public IEnumerator SparkImpact(float duration)
    {
        var emissionComponent = impactSparks.emission;
        emissionComponent.enabled = true;
        yield return new WaitForSeconds(duration);
        emissionComponent.enabled = false;
    }
    public void ClearList()
    {
        enemiesHit.Clear();
    }
}
