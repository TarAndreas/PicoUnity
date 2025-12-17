using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Once trigger is left, set GameObject active or inactive
 */

public class LeaveTriggerArea : MonoBehaviour
{
    [SerializeField]
    public GameObject ObjectToActivate;

    [SerializeField]
    public GameObject ObjectToDeactivate;

    public void OnTriggerExit(Collider other)
    {
        ObjectToActivate.SetActive(true);
        ObjectToDeactivate.SetActive(false);
        Debug.Log("Leaving Trigger Area/Room");


    }
}
