using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovement : MonoBehaviour
{
    public Transform mainCamera;
    public float speed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.W))
        {
            transform.position += mainCamera.forward * speed* Time.deltaTime;
        }
        else if ((Input.GetKey(KeyCode.S)))
        {
            transform.position -= mainCamera.forward * speed * Time.deltaTime;

        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= mainCamera.right * speed * Time.deltaTime;
        }
        else if ((Input.GetKey(KeyCode.D)))
        {
            transform.position += mainCamera.right * speed * Time.deltaTime;

        }
    }
}
