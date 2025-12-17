using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class ConnectionHandlerRooms : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    if(!PhotonNetwork.IsConnected)
    //    {
    //        OnLeftRoom();
    //    }
    //}

    public void LeavingRoom()
    {
        //base.OnEnable();
        //PhotonNetwork.Disconnect();

        PhotonNetwork.LeaveRoom();
        //PhotonNetwork.LeaveLobby();
        Debug.Log("Leaving the room");
        OnLeftRoom();
    }

    /*
    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.Disconnect();
        Debug.Log("Enabled Disconnect");
    }
    */

    //public override void OnLeftRoom()
    //{
    //    base.OnLeftRoom();
    //    PhotonNetwork.Disconnect();
    //    PhotonNetwork.LoadLevel(0);
    //    Debug.Log("Disconnected");
    //}

    //public override void OnDisconnected(DisconnectCause cause)
    //{
    //    base.OnDisconnected(cause);
    //    PhotonNetwork.LoadLevel(0);
    //    Debug.Log("Loading Home scene");
    //}
    
    public void ShowRoomName()
    {

        TMP_Text roomName;
        PhotonNetwork.CurrentRoom.Name.ToString();
    }


}
