using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdMovAnimation : MonoBehaviour
{
    public string stopVarName = "Stop";
    public float stopThresHold = .1f;
    public float swerveLerp = 2;
    public Animator animator;
    public Rigidbody body;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        animator.SetBool(stopVarName, body.velocity.magnitude < stopThresHold);
        transform.forward = Vector3.Lerp(transform.forward, body.velocity.normalized, Time.deltaTime * swerveLerp);


    }
}
