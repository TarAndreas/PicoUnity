using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TestRoomHandler : MonoBehaviourPunCallbacks
{
    private int SceneIndexNumber = 10;
    public List<DefaultRoom> defaultRooms;

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
            PhotonNetwork.JoinLobby();
            Debug.Log("Joining a lobby...");
            //Custom Lobby possible
            //PhotonNetwork.JoinLobby(customLobby);
        }
    }

    public override void OnJoinRandomFailed(short returncode, string message)
    {
        base.OnJoinRandomFailed(returncode, message);
        Debug.Log(message);

    }

    public override void OnJoinRoomFailed(short returncode, string message)
    {
        base.OnJoinRoomFailed(returncode, message);
        Debug.Log(message);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.Log("A room is created with name: " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Connected to Lobby again");
        PhotonNetwork.JoinLobby();

    }

    public override void OnCreateRoomFailed(short returncode, string message)
    {
        base.OnCreateRoomFailed(returncode, message);
        Debug.Log("Failed to create a room: " + message, this);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnJoinedRoom();
        Debug.Log(newPlayer.NickName + " joined to:" + "Player count " + PhotonNetwork.CurrentRoom.PlayerCount);
        base.OnPlayerEnteredRoom(newPlayer);
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("Joined a Lobby");
    }

    public override void OnLeftLobby()
    {
        base.OnLeftLobby();
        Debug.Log("Left the Lobby");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined a Room: " + PhotonNetwork.CurrentRoom.Name);


        LoadSelectedScene();


    }


    public void InitializeRoom(int defaultRoomIndex)
    {
        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];

        //Create Room
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)roomSettings.maxPlayer;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

        PhotonNetwork.JoinOrCreateRoom(roomSettings.Name, roomOptions, TypedLobby.Default);
        Debug.Log("Creating a room: " + roomSettings.Name);

        SceneIndexNumber = roomSettings.sceneIndex;

    }


    public void LoadSelectedScene()
    {
        //Load Scene
        Debug.Log("SceneIndexNumber: " + SceneIndexNumber);

        if (SceneIndexNumber != 10)
        {
            Debug.Log("Index Number accepted");
            PhotonNetwork.LoadLevel(SceneIndexNumber);

        }
        else
        {
            Debug.Log("Couldnt load a scene, index out of bound");
        }
    }
}
