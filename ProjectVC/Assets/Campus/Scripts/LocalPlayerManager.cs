using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/*
 * Check USB
 */

public class LocalPlayerManager : MonoBehaviourPunCallbacks
{
    public static GameObject LocalPlayerInstance;
    private PhotonView localPhotonView;

    void Awake()
    {

        localPhotonView = GetComponent <PhotonView>();

        if (photonView.IsMine)
        {
            LocalPlayerManager.LocalPlayerInstance = this.gameObject;

        } else
        {
            Debug.Log("PhotonView is not mine!");
        }

        DontDestroyOnLoad(this.gameObject);
    }

    
}
