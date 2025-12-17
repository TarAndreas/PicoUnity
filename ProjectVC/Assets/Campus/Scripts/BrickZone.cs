/*
 * Objects with a predifined tag that enter a trigger zone, change the boolean value
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickZone : MonoBehaviour
{

    public string TriggerName = "";
    private bool bCheckZone = false;
    public SnapZoneAction sza;

    //Change boolean value once an object entered the trigger
    public void OnTriggerEnter(Collider snapzone)
    {
        if(snapzone.tag == TriggerName)
        {
            bCheckZone = true;
            //Function
            sza.BrickOnSnapZone(bCheckZone);
            Debug.Log("Trigger name and tag are the same");
        }

    }
    //Change boolean value once an object left the trigger
    public void OnTriggerExit(Collider snapzone)
    {
        if(snapzone.tag == TriggerName) {

            bCheckZone = false;

            //Function
            sza.BrickOnSnapZone(bCheckZone);
            Debug.Log("Left Snap Zone");
        } else
        {
            bCheckZone = true;
        }



    }
}
