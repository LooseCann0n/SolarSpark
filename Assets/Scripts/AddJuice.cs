using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddJuice : MonoBehaviour
{
    public GameObject UIText;
    private SimpleCombat combatHeat;
    private bool TakenDip;

    private void Start()
    {
        UIText.SetActive(false);
        TakenDip = false;
        combatHeat = GameObject.FindGameObjectWithTag("Player").GetComponent<SimpleCombat>();
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            combatHeat.IncreaseHeat(150);
            if (TakenDip == false)
            {
                StartCoroutine(ActivateText());
                TakenDip = true;
            }
        }
    }
    IEnumerator ActivateText()
    {
        UIText.SetActive(true);
        yield return new WaitForSeconds(3);
        UIText.SetActive(false);
    }
}
