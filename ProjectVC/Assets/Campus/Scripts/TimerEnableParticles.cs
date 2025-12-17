using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Assign what particles should be enabled once the timmer hits 0
/// </summary>
public class TimerEnableParticles : MonoBehaviour
{
    [SerializeField]
    public float StartTime;

    [SerializeField]
    public bool activate = false;

    [SerializeField]
    public GameObject objectEmitter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (StartTime > 0)
        {
            StartTime -= Time.deltaTime;
        }
        else if (activate)
        {
            activateEmitter();
        }
        else
        {
            deactivateEmitter();
        }
    }

    public void activateEmitter()
    {
        objectEmitter.SetActive(true);
    }

    public void deactivateEmitter()
    {
        objectEmitter.SetActive(false);
    }
}
