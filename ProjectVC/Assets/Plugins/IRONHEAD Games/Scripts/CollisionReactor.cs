using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionReactor : MonoBehaviour, ICollisionReceiver
{
    [SerializeField]
    GameObject door;
    bool isOpened = false;

    public void OnTriggerEnterNotif(Collider other)
    {

        if (!isOpened)
        {
            isOpened = true;

            door.transform.position -= new Vector3(0, 4, 0);
        }
    }



    public void OnTriggerExitNotif(Collider other)
    {
    }

    public void OnTriggerStayNotif(Collider other)
    {
    }
}