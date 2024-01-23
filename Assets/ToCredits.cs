using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToCredits : MonoBehaviour
{
    private SimpleEnemy sobek;
    [SerializeField] private Animator animator;
    private bool triggered;

    private void Start()
    {
        sobek = GameObject.Find("Sobek").GetComponent<SimpleEnemy>();
    }

    void Update()
    {
        if (sobek.Health <= 0 && triggered == false)
        {

            triggered = true;
        }
    }


}
