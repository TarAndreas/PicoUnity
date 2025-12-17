using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Destroy assigned GameObject once the set time hits 0
/// </summary>

public class TimerDestroyObject : MonoBehaviour
{

    [SerializeField]
    public float StartTime;

    public bool timerIsRunning = false;

    public GameObject destroyThisGameObject;

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
            else
            {
                DestroyAnObject();
            }
        }
    }

    public void DestroyAnObject()
    {
        Destroy(destroyThisGameObject);
    }
}
