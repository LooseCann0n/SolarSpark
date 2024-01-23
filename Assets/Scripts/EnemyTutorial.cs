using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTutorial : MonoBehaviour
{
    public GameObject Enemy1;
    public GameObject Enemy2;
    public GameObject Enemy3;
    private SimpleCombat combatHeat;
    private bool entered;

    private void Start()
    {
        combatHeat = GameObject.FindGameObjectWithTag("Player").GetComponent<SimpleCombat>();
    }

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player" && entered == false)
        {
            combatHeat.IncreaseHeat(100);
            Enemy1.SetActive(true);
            Enemy2.SetActive(true);
            Enemy3.SetActive(true);
            entered = true;
        }
    }
}
