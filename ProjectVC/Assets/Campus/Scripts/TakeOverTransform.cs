using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Once the GameObject enters the trigger area, assign its position to the child object
/// </summary>
public class TakeOverTransform : MonoBehaviour
{

    public Transform GOOrigin;
    public Transform GOChild;
    bool OriginEntered = false;


    // Update is called once per frame
    void Update()
    {
        if(OriginEntered == true)
        {
            GOChild.position = GOOrigin.position;
        }
        
    }

    public void OnTriggerEnter(Collider other)
    {
        GOOrigin = other.transform;
        OriginEntered = true;
    }
}
