/*
 * Connection Manager for the Photon servers
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.Events;

public class ConnectToNetwork : MonoBehaviourPunCallbacks
{
    public TMP_InputField Inputfield_Name;

    public static UnityEvent<Player> OnPlayerLeft;

    // Start is called before the first frame update
    void Awake()
    {
       

        OnPlayerLeft = new UnityEvent<Player>();
    }

    public void ConnectToServer()
    {
        if (Inputfield_Name != null)
        {
            PhotonNetwork.NickName = Inputfield_Name.text;

            if(!PhotonNetwork.IsConnected)
                PhotonNetwork.ConnectUsingSettings();
            Debug.Log("Try Connect to Server...");
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Server...");
        base.OnConnectedToMaster();

    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        OnPlayerLeft?.Invoke(otherPlayer);
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
    }
}
