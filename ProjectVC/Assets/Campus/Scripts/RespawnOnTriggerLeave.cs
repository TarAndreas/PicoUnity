using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/*
 * Instantiate GameObject once trigger area is entered
 * OBSOLETE SCRIPT
 */
public class RespawnOnTriggerLeave : MonoBehaviour
{

    public GameObject RespawnObj;
    public Transform spawnPos;
    public GameObject firstObj;
    private GameObject newObj;


    private void OnTriggerExit(Collider other)
    {

        if(other.gameObject == firstObj)
        {
            //newObj = Instantiate(RespawnObj, spawnPos);
            TriggerObjRPC();
        }

        //firstObj = newObj;
        
    }

    [PunRPC]
    private void TriggerObjInstantiate()
    {
        newObj = Instantiate(RespawnObj, spawnPos);
        firstObj = newObj;
    }

    private void TriggerObjRPC()
    {
        PhotonView myPV = PhotonView.Get(this);
        myPV.RPC("TriggerObjInstantiate", RpcTarget.All);
    }
}
