using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wayPoint : MonoBehaviour
{
    public List<Transform> waypoints = new List<Transform>();
    private Transform targetWayPoint;
    private int targetWaypointIndex = 0;
    private float minDistance = 0.1f;
    private int lastWayointIndex;
    float movementSpeed = 1.0f;
    private float rotationSpeed = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        lastWayointIndex = waypoints.Count -1;
        targetWayPoint = waypoints[targetWaypointIndex];

        
    }

    // Update is called once per frame
    void Update()
    {
        float movementSteps = movementSpeed * Time.deltaTime;
        float rotationSteps = rotationSpeed * Time.deltaTime;
        Vector3 directionToTarget = targetWayPoint.position - transform.position;

        Quaternion rotationToTarget = Quaternion.LookRotation(directionToTarget);

        transform.rotation = rotationToTarget;

        float distance = Vector3.Distance(transform.position, targetWayPoint.position);
        CheckDistanceToWaypoint(distance);
        Debug.Log("Distance:" + distance);

        transform.position = Vector3.MoveTowards(transform.position, targetWayPoint.position, movementSteps);

        
    }

    void CheckDistanceToWaypoint(float currentDistance)
    {
        if (currentDistance <= minDistance)
        {
            targetWaypointIndex ++;
            UpdateTargetWaypoint();
        }

    }

    void UpdateTargetWaypoint()
    {
        if (targetWaypointIndex > lastWayointIndex)
        {
            targetWaypointIndex = 0;
        }
        targetWayPoint = waypoints[targetWaypointIndex];
    }

}
