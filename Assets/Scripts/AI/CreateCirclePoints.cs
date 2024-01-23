using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCirclePoints : MonoBehaviour
{
    public List<Transform> circlePoints = new List<Transform>();
    public Transform center;
    public GameObject objectToSpawn;
    public float radius = 5f;
    public int points = 8;

    private void Start()
    {
        CreateCirclePositions(points, center.position, radius);
    }

    public void CreateCirclePositions(int num, Vector3 point, float radius)
    {
        for (int i = 0; i < num; i++)
        {
            /* Distance around the circle */
            var radians = i * 2 * Mathf.PI / num;

            /* Get the vector direction */
            var vertical = Mathf.Sin(radians);
            var horizontal = Mathf.Cos(radians);

            var spawnDir = new Vector3(horizontal, 0, vertical);

            /* Get the spawn position */
            var spawnPos = point + spawnDir * radius; // Radius is just the distance away from the point

            /* Now spawn */
            var circlePoint = Instantiate(objectToSpawn, spawnPos, Quaternion.identity, transform) as GameObject;

            /* Rotate the enemy to face towards player */
            circlePoint.transform.LookAt(point);

            /* Adjust height */
            circlePoint.transform.Translate(new Vector3(0, circlePoint.transform.localScale.y / 2, 0));
            circlePoints.Add(circlePoint.transform);
        }
    }
}
