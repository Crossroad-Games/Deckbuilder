using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bezier : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform point0, point1, point2, point3;

    private int numPoints = 50;
    private Vector3[] positions = new Vector3[50];

    
    // Start is called before the first frame update
    void Start()
    {
        //lineRenderer.SetVertexCount(numPoints);
        lineRenderer.positionCount = numPoints;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DrawCubicCurve()
    {
        for(int i = 1; i < numPoints; i++)
        {
            float t = i / (float)numPoints;
            if(CalculateCubicBezierPoint(t, point0.position, point1.position, point2.position, point3.position).magnitude != 0)
                positions[i - 1] = CalculateCubicBezierPoint(t, point0.position, point1.position, point2.position,point3.position);
        }
        positions[49] = positions[48];
        lineRenderer.SetPositions(positions);
    }

    private Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;
        return p;
    }

    private Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;
        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;
        return p;
    }
}
