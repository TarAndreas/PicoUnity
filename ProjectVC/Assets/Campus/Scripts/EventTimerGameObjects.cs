/*
 * Activate GameObjects once timer hits 0
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTimerGameObjects : MonoBehaviour
{
    [SerializeField]
    public float StartTime;

    public bool timerIsRunning = false;

    [SerializeField]
    public GameObject activeObject;

    public bool ActivateObject = true;

    // Start is called before the first frame update
    void Start()
    {
        timerIsRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerIsRunning)
        {
            if (StartTime > 0)
            {
                StartTime -= Time.deltaTime;
            }
            else if(ActivateObject == false)
            {
                deactivateObject();

            } else
            {
                activateObject();
            }
        }
    }

    public void activateObject()
    {
        activeObject.SetActive(true);
    }

    public void deactivateObject()
    {
        activeObject.SetActive(false);
    }
}
