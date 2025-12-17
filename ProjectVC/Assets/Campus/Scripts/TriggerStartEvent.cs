using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Obsolete class
public class TriggerStartEvent : MonoBehaviour
{
    [System.Serializable]
    public class EventTriggerExecute : UnityEvent<int> { }
    public EventTriggerExecute ExecuteOnTrigger;

}
