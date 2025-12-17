using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBroadcaster : MonoBehaviour
{
    [SerializeField] public ICollisionReceiver triggerReceiver;

    private void OnTriggerEnter(Collider other)
    {
        triggerReceiver.OnTriggerEnterNotif(other);
    }

    private void OnTriggerStay(Collider other)
    {
        triggerReceiver.OnTriggerStayNotif(other);
    }

    private void OnTriggerExit(Collider other)
    {
        triggerReceiver.OnTriggerExitNotif(other);
    }
}
