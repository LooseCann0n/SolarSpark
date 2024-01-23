using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAttackLocations : MonoBehaviour
{
    public int numberOfLocations;
    public float radiusDirection;
    public float distanceFromPlayer;

    private Transform player;
    private UnityEngine.Animations.PositionConstraint posConstraint;
    private bool validPosition;
    [SerializeField]
    private int collisonCount = 0;

    private Vector3 currentAttackPos;
    public Transform positionHolder;
    public GameObject attackPosition;
    public LayerMask attackPosLayer;

    public List<AttackPosition> attackPositions = new List<AttackPosition>();

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        posConstraint = GetComponent<UnityEngine.Animations.PositionConstraint>();

        for (int i = 0; i < numberOfLocations; i++)
        {
            validPosition = false;
            collisonCount = 0;
            Vector3 circlePosition = RandomPointOnCircleEdge(distanceFromPlayer);
            if (attackPositions.Count > 0)
                circlePosition = CheckIfClear(circlePosition);

            Physics.SyncTransforms();
            GameObject position = Instantiate(attackPosition, circlePosition, Quaternion.identity, positionHolder);
            attackPositions.Add(position.GetComponent<AttackPosition>());
            
        }
        if (attackPositions.Count == numberOfLocations)
        {
            posConstraint.constraintActive = true;
            for (int j = 0; j < numberOfLocations; j++)
            {
                attackPositions[j].GetComponent<BoxCollider>().enabled = false;
            }
        }


    }

    private Vector3 CheckIfClear(Vector3 circleEdge)
    {       
        if(!validPosition)
        {
            Physics.SyncTransforms();
            Collider[] hitAttackPositions = Physics.OverlapSphere(circleEdge, radiusDirection, attackPosLayer);
            Physics.SyncTransforms();

            if (hitAttackPositions.Length > 0)
            {
                circleEdge = RandomPointOnCircleEdge(distanceFromPlayer);
                CheckIfClear(circleEdge);
            }
            else
            {               
                validPosition = true;
                return circleEdge;
            }
        }
        return circleEdge;
    }

    private Vector3 RandomPointOnCircleEdge(float radius)
    {
        var vector2 = Random.insideUnitCircle.normalized * radius;
        return new Vector3(vector2.x, 0, vector2.y);
    }
}
