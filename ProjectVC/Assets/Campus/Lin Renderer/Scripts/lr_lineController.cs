using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lr_lineController : MonoBehaviour
{
    private LineRenderer lr;
    private Transform[] points;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }
    public void SetUpLine(Transform[] waypoints)
    {
        lr.positionCount = waypoints.Length;
        points = waypoints;
    }

    void Update()
    {

        for (int i = 0; i < points.Length; i++)
        {
            lr.SetPosition(i, points[i].position);

        }
        
    }

}
