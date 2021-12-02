using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayEndPointGetter : MonoBehaviour
{
    LineRenderer lineRenderer;
    public Transform endPoint;
    Vector3[] points = new Vector3[200];
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        var numPoints = lineRenderer.GetPositions(points);

        if (numPoints > 0)
            endPoint.position = points[numPoints - 1];

        endPoint.gameObject.SetActive(false);
    }
    
}
