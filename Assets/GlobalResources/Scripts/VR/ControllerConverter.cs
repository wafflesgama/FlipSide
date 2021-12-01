using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerConverter : MonoBehaviour
{
    public Transform controllerSource;
    public bool invertRotation = true;
    public bool followPos = true;
    public bool followRot = true;
    public bool invertPosition = false;
    public Vector3 posOffset;
    public Vector3 rotOffset;


    // Update is called once per frame
    void Update()
    {
        if (followPos)
            transform.position = (invertPosition ? -1 : 1) * controllerSource.position + posOffset;
        
        if (followRot)
            transform.eulerAngles = new Vector3(controllerSource.eulerAngles.x * (invertRotation ? -1 : 1), controllerSource.eulerAngles.y, controllerSource.eulerAngles.z * (invertRotation ? -1 : 1)) + rotOffset;
    }
}
