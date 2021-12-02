using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VrMovementController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Teleport(Vector3 pos)
    {
        transform.position = new Vector3(pos.x,transform.position.y,pos.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
