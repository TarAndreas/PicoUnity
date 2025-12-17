/*
 * Fire event once the defined time hits 0
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventTimer : MonoBehaviour
{

    [SerializeField]
    public float StartTime;

    //[System.Serializable]
    //public class TimerEvent : UnityEvent<int> { }
    public UnityEvent OnTimerEnd;

    public bool timerIsRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        timerIsRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(timerIsRunning)
        {
            if(StartTime > 0)
            {
                StartTime -= Time.deltaTime;
            } else
            {
                OnTimerEnd.Invoke();

            }
        }
    }

}



