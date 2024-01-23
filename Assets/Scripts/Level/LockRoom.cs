using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRoom : MonoBehaviour
{
    public Transform player;
    private Animator doorController;
    private bool hasToggled = false;

    [Tooltip("List of all enemies attached to this door")]
    public List<SimpleEnemy> enemies = new List<SimpleEnemy>();
    [Tooltip("Is the door open")]
    public bool doorOpen;
    [Header("Door Bools")]

    [Space(10)]
    [Tooltip("If true door starts open")]
    public bool startOpen;
    [Tooltip("Door closes when player enters box collider ")]
    public bool closeOnEntry;
    [Tooltip("Door open once all enemies in list above are dead")]
    public bool openOnRoomClear;
    [Tooltip("Open (or close) door when player enters the box collider")]
    public bool toggleStateOnEntry;
    
    [Tooltip("Door will open once a certain event is true")]
    public bool openOnEvent;

    private void Start()
    {
        doorController = GetComponent<Animator>();
        if (startOpen == true)
            doorOpen = true;
        if (openOnRoomClear == true && enemies.Count == 0)
            Debug.LogWarning("You have specified you want door to open on enemy clear but have assigned no enemies to the list above", gameObject);
    }

    private void Update()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].enabled == false)
                enemies.Remove(enemies[i]);
        }
        if (enemies.Count == 0)
        {
            if (openOnRoomClear == true)
            {
                if (doorOpen == false)
                    OpenDoor();
            }
        }
        if (openOnEvent == true)
        {
            if (GetComponent<LeverOpen>().openDoor == true)
            {
                OpenDoor();
                if (doorOpen)
                    openOnEvent = false;
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (closeOnEntry == true)
            {
                CloseDoor();
                closeOnEntry = false;
            }
            if (toggleStateOnEntry == true)
            {
                if (doorOpen == true && hasToggled == false)
                    CloseDoor();
                if (doorOpen == false && hasToggled == false)
                    OpenDoor();
                hasToggled = false;
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (toggleStateOnEntry == true)
        {
            if (doorOpen == true && hasToggled == false)
                CloseDoor();
            if (doorOpen == false && hasToggled == false)
                OpenDoor();
            hasToggled = false;
        }
    }

    public void OpenDoor()
    {
        Debug.Log("Open door");
        doorOpen = true;
        doorController.Play("OpenDoor");
        hasToggled = true;
    }

    public void CloseDoor()
    {
        Debug.Log("Close door");
        doorOpen = false;
        doorController.Play("CloseDoor");
        hasToggled = true;
    }
}
