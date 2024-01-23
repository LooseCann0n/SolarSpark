using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIManager : MonoBehaviour
{
    private static AIManager instance;
    public static AIManager Instance
    {
        get 
        { 
            return instance; 
        }
        private set 
        { instance = value; 
        }
    }

    [SerializeField]
    private int maximumAttackers;
    [SerializeField]
    public int activeAttackers;

    public List<Transform> enemies = new List<Transform>();

    public float radiusAroundTarget = 0.5f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            return;
        }
        Destroy(gameObject);
    }

    public bool AttackSpotsAvailable()
    {
        if (activeAttackers < maximumAttackers)
        {
            return true;
        }
        return false;
    }
}
