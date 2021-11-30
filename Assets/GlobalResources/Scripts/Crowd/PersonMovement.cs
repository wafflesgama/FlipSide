using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(CharacterController))]
public class PersonMovement : MonoBehaviour
{
    public float moveSpeed;
    public float rotLerpSpeed;

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
        if ((upDirection && currentWayPoint >= waypoints.Length - 1) || (!upDirection && currentWayPoint <= 0))
        {
            isMoving = false;
            return;
        }
        nextWayPoint += upDirection ? 1 : -1;
    }

    void FixedUpdate()
    {
        if (!isMoving) return;

        if (Vector3.Distance(waypoints[nextWayPoint].position, transform.position) < 2f)
            SetNextWaypoint();


        transform.forward = Vector3.Lerp(transform.forward, waypoints[nextWayPoint].position - transform.position, Time.deltaTime * rotLerpSpeed);

        rigidBody.AddForce(transform.forward * moveSpeed);
        //characterController.Move(transform.forward * moveSpeed);
    }
}
