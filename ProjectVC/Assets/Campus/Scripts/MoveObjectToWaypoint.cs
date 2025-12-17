using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GameObject changes position according to the waypoints set. Has option to return to previous position
/// </summary>
public class MoveObjectToWaypoint : MonoBehaviour
{
    public GameObject[] waypoints;
    public GameObject[] startpos;
    int current = 0;
    public float speed;
    float WPradius = 1;
    public bool forward = true;
    public bool movementActivated = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (forward == true && movementActivated == true)
        {
            MoveToWaypoint();
        }
        else if(forward == false && movementActivated == true)
        {
            ReturnToStartPosition();
        }
    }

    public void MoveToWaypoint()
    {
        if (Vector3.Distance(waypoints[current].transform.position, transform.position) < WPradius)
        {
            current++;
            if (current >= waypoints.Length)
            {
                current = 0;
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, waypoints[current].transform.position, Time.deltaTime * speed);
        
        if (Vector3.Distance(transform.position, waypoints[current].transform.position) < .001f)
        {
            forward = false;
            movementActivated = false;
        }
    }

    public void ReturnToStartPosition()
    {
        if (Vector3.Distance(startpos[current].transform.position, transform.position) < WPradius)
        {
            current++;
            if (current >= waypoints.Length)
            {
                current = 0;
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, startpos[current].transform.position, Time.deltaTime * speed);
        
        if(Vector3.Distance(transform.position, startpos[current].transform.position) < .001f) {
            forward = true;
            movementActivated = false;
        }
    }

    public void ActivateMovement()
    {
        movementActivated = true;
    }

}
