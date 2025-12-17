using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLogs : MonoBehaviour
{

    //Console output on GameObject entered the trigger
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Object: " + other.gameObject);
    }
}
