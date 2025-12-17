/*
 * Player spawn in current room, if connected to a Photon server
 * Player count is increased
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class EnterRoomHandler : MonoBehaviourPunCallbacks
{
    public Vector3 spawnPosition;
    [SerializeField]
    GameObject VRPlayerPrefab;

    [SerializeField]
    GameObject FPPlayerPrefab;

    private bool instantiated = false;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        if (!PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.ConnectUsingSettings();
            Debug.Log("Connect to the Network");

        }

        else
        {
            InstantiatePlayers();
            
        }
    }

    public override void OnJoinRoomFailed(short returncode, string message)
    {
        base.OnJoinRoomFailed(returncode, message);
        Debug.Log(message);

    }

    
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnJoinedRoom();
        Debug.Log(newPlayer.NickName + " joined to:" + "Player count " + PhotonNetwork.CurrentRoom.PlayerCount);
        base.OnPlayerEnteredRoom(newPlayer);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined a Room");

        if(!instantiated)
        {
            InstantiatePlayers();
        }

    }

    //Spawn Player XRRig
    public void InstantiatePlayers()
    {
        PhotonNetwork.Instantiate(this.VRPlayerPrefab.name, spawnPosition, Quaternion.identity);
        Debug.Log("Instanting the joined Player");
        instantiated = true;
    }

}
