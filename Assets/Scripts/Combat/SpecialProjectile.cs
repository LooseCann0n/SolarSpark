using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialProjectile : MonoBehaviour
{
    int m_damage;
    float m_speed;
    float m_timeToDestroy;

    List<GameObject> closedList;

    private void Update()
    {
        transform.Translate(Vector3.forward * m_speed * Time.deltaTime);
    }

    public void SetVariables(int damage, float speed, float timeToDestroy)
    {
        m_damage = damage;
        m_speed = speed;
        m_timeToDestroy = timeToDestroy;
        StartCoroutine(selfDestruct());
        closedList = new List<GameObject>();
    }

    void DealDamage(SimpleEnemy enemy)
    {
        //enemy.TakeDamage(m_damage);
        IDamageable damageable = enemy.GetComponent<IDamageable>();
        // Object has no damageable component, do exit
        if (damageable == null)
            return;
        damageable.Damage(m_damage);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") && !closedList.Contains(other.gameObject))
        {
            if (!other.gameObject.CompareTag("Enemy"))
                other.GetComponent<IDamageable>().Damage(m_damage);
            SimpleEnemy enemyComponent = other.gameObject.GetComponent<SimpleEnemy>();
            closedList.Add(other.gameObject);
            DealDamage(enemyComponent);
        }
    }
    IEnumerator selfDestruct()
    {
        yield return new WaitForSeconds(m_timeToDestroy);
        Destroy(gameObject);
    }
}
