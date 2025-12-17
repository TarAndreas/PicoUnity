using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerCollisionDetection : MonoBehaviour
{
    public string triggerCollisionTag;

    public UnityEvent<GameObject> OnTriggerEntered = new UnityEvent<GameObject>();
    public UnityEvent<GameObject> OnTriggerExited = new UnityEvent<GameObject>();


    private void OnTriggerEnter(Collider other)
    {
        if (other.name == triggerCollisionTag)
        {
            OnTriggerEntered.Invoke(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == triggerCollisionTag)
        {
            OnTriggerExited.Invoke(other.gameObject);
        }
    }


}
