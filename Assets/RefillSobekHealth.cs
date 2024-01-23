using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefillSobekHealth : MonoBehaviour
{
    private PlayerManager playerManager;
    public GameObject sobek;
    private SimpleEnemy sobekEnemy;

    // Start is called before the first frame update
    void Start()
    {
        playerManager = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
        sobekEnemy = sobek.GetComponent<SimpleEnemy>();
    }

    void Update()
    {
        if (playerManager.alive == false)
        {
            sobekEnemy.Health = 1400;
        }
    }
}
