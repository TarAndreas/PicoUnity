/*
 * Activate every GameObject inside the eventList and deactivate every GameObject in the deactivateList
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivInactiveList : MonoBehaviour
{
    public List<GameObject> eventList;
    public List<GameObject> deactivateList;

    //Execute function once trigger is entered
    public void OnTriggerEnter(Collider other)
    {
        ExeEventList();
    }
    //Clear lists once trigger is left
    public void OnTriggerExit(Collider other)
    {
        eventList.Clear();
        deactivateList.Clear();
    }
    //Activate or deactivate objects in the list
    public void ExeEventList()
    {
        foreach (GameObject gobj in eventList)
        {
            gobj.SetActive(true);
        }

        foreach (GameObject deactiveGO in deactivateList)
        {
            deactiveGO.SetActive(false);
        }
    }

}
