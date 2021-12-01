using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(CharacterController))]
public class PersonMovement : MonoBehaviour
{
    public float moveSpeed;
    public float rotLerpSpeed;
    public float swerveForce;
    public float nextWaypointThres = 2.5f;
    public float swerveThres = .4f;

    public bool isMoving;
    public bool upDirection;
    public Transform[] waypoints;
    public int currentWayPoint;
    public int nextWayPoint;

    //CharacterController characterController;
    Rigidbody rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        //characterController = GetComponent<CharacterController>();
    }

    public void Spawn(bool upDirection, Transform[] waypoints, int currentWayPoint)
    {

        this.upDirection = upDirection;
        this.waypoints = waypoints;
        this.currentWayPoint = currentWayPoint;
        this.nextWayPoint = currentWayPoint;
        transform.position = waypoints[currentWayPoint].position;
        SetNextWaypoint();
        isMoving = true;

    }

    void SetNextWaypoint()
    {
        if ((upDirection && nextWayPoint >= waypoints.Length - 1) || (!upDirection && currentWayPoint <= 0))
        {
            isMoving = false;
            Destroy(gameObject);
            return;
        }
        nextWayPoint += upDirection ? 1 : -1;
    }

    void FixedUpdate()
    {
        if (!isMoving) return;

        if (Vector3.Distance(waypoints[nextWayPoint].position, transform.position) < nextWaypointThres)
            SetNextWaypoint();

        if(rigidBody.velocity.magnitude < swerveThres)
        {
            Vector3 direction;
            if (upDirection)
                direction = transform.right;
            else
                direction = -transform.right;

            rigidBody.AddForce(direction * swerveForce, ForceMode.Impulse);
        }

        transform.forward = Vector3.Lerp(transform.forward, waypoints[nextWayPoint].position - transform.position, Time.deltaTime * rotLerpSpeed);

        rigidBody.AddForce(transform.forward * moveSpeed);
        //characterController.Move(transform.forward * moveSpeed);
    }
}
