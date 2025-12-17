using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollisionReceiver
{
    void OnTriggerEnterNotif(Collider other);
    void OnTriggerStayNotif(Collider other);
    void OnTriggerExitNotif(Collider other);

}
