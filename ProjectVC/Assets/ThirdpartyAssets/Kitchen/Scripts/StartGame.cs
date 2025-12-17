using JetBrains.Annotations;
using System.Collections;
//using JetBrains.Annotations;
//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Unity.VisualScripting;


public class StartGame : MonoBehaviour
{


    [SerializeField] public TextMeshPro ShowTimer;
    [SerializeField] public GameObject rezept;
    [SerializeField] public float StartTime;
    [System.Serializable]
    public class TimerEvent : UnityEvent<int> { }
    public TimerEvent OnTimerEnd;

    public bool timerIsRunning = false;

    [SerializeField]
    public GameObject objectEmitter;


    void Update()
    {
        if (timerIsRunning)
        {

            if (StartTime > 0)
            {
                StartTime -= Time.deltaTime;
                ShowTimer.text = StartTime.ToString();
            }
            else
            {
                
            }
        }
    }
    public void GameStart()
    {
        timerIsRunning = true;
    }

}
