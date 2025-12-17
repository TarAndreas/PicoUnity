using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    TextMeshProUGUI OccupancyRateText_ForSchool;

    [SerializeField]
    TextMeshProUGUI OccupancyRateText_ForOutdoor;

    [SerializeField]
    TextMeshProUGUI OccupancyRateText_ForLabyrinth;

    [SerializeField]
    TextMeshProUGUI OccupancyRateText_ForBesprechungsraum;

    string mapType;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            PhotonNetwork.JoinLobby();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region UI Callback Methods
    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnEnterRoomButtonClicked_Outdoor()
    {
        mapType = MultiplayerVRConstants.MAP_TYPE_VALUE_OUTDOOR;
        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { MultiplayerVRConstants.MAP_TYPE_KEY, mapType } };
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 4);
    }

    public void OnEnterRoomButtonClicked_School()
    {
        mapType = MultiplayerVRConstants.MAP_TYPE_VALUE_SCHOOL;
        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { MultiplayerVRConstants.MAP_TYPE_KEY, mapType } };
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 4);
    }

    public void OnEnterRoomButtonClicked_Labyrinth()
    {
        mapType = MultiplayerVRConstants.MAP_TYPE_VALUE_LABYRINTH;
        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { MultiplayerVRConstants.MAP_TYPE_KEY, mapType } };
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 4);
    }

    public void OnEnterRoomButtonClicked_Besprechungsraum()
    {
        mapType = MultiplayerVRConstants.MAP_TYPE_VALUE_BESPRECHUNGSRAUM;
        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { MultiplayerVRConstants.MAP_TYPE_KEY, mapType } };
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 4);
    }

    #endregion

        #region Photon Callbacks

    public override void OnJoinRandomFailed(short returncode, string message)
    {
        //base.OnJoinRandomFailed( returncode, message);
        Debug.Log(message);
        CreateAndJoinRoom();
    }

    public override void OnCreatedRoom()
    {
        //        base.OnCreatedRoom();
        Debug.Log("A room is created with name:" + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to servers again");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedRoom()
    {
        //base.OnJoinedRoom();
        Debug.Log("The local Player " + PhotonNetwork.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name + " Player count " + PhotonNetwork.CurrentRoom.PlayerCount);
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(MultiplayerVRConstants.MAP_TYPE_KEY))
        {
            object mapType;
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(MultiplayerVRConstants.MAP_TYPE_KEY, out mapType))
            {
                Debug.Log("Joined Room with the map: " + (string)mapType);
                if ((string)mapType == MultiplayerVRConstants.MAP_TYPE_VALUE_SCHOOL)
                {
                    PhotonNetwork.LoadLevel("World_School");
                }
                else if ((string)mapType == MultiplayerVRConstants.MAP_TYPE_VALUE_OUTDOOR)
                {
                    PhotonNetwork.LoadLevel("World_Outdoor");
                }
                else if ((string)mapType == MultiplayerVRConstants.MAP_TYPE_VALUE_LABYRINTH)
                {
                    PhotonNetwork.LoadLevel("test");
                }

                else if ((string)mapType == MultiplayerVRConstants.MAP_TYPE_VALUE_BESPRECHUNGSRAUM)
                {
                    PhotonNetwork.LoadLevel("Besprechungsraum");
                }
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //base.OnJoinedRoom();
        Debug.Log(newPlayer.NickName + " joined to:" + "Player count " + PhotonNetwork.CurrentRoom.PlayerCount);
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (roomList.Count == 0)
        {
            //no room yet
            OccupancyRateText_ForSchool.text = 0 + " / " + 4;
            OccupancyRateText_ForOutdoor.text = 0 + " / " + 4;
            OccupancyRateText_ForLabyrinth.text = 0 + " / " + 4;
            OccupancyRateText_ForBesprechungsraum.text = 0 + " / " + 4;
        }
        foreach (RoomInfo room in roomList)
        {
            Debug.Log(room.Name);
            if (room.Name.Contains(MultiplayerVRConstants.MAP_TYPE_VALUE_SCHOOL))
            {
                //update the School map
                // Debug.Log("Room is a School Map. Player Count is " + room.PlayerCount);
                OccupancyRateText_ForSchool.text = room.PlayerCount + " / " + 4;
            }
            else if (room.Name.Contains(MultiplayerVRConstants.MAP_TYPE_VALUE_OUTDOOR))
            {
                //update the Outdoor map
                // Debug.Log("Room is a Outdoor Map. Player Count is " + room.PlayerCount);
                OccupancyRateText_ForOutdoor.text = room.PlayerCount + " / " + 4;
            }
            else if (room.Name.Contains(MultiplayerVRConstants.MAP_TYPE_VALUE_LABYRINTH))
            {
                //update the Outdoor map
                // Debug.Log("Room is a Outdoor Map. Player Count is " + room.PlayerCount);
                OccupancyRateText_ForLabyrinth.text = room.PlayerCount + " / " + 4;
            }
            else if (room.Name.Contains(MultiplayerVRConstants.MAP_TYPE_VALUE_BESPRECHUNGSRAUM))
            {
                //update the Outdoor map
                // Debug.Log("Room is a Outdoor Map. Player Count is " + room.PlayerCount);
                OccupancyRateText_ForBesprechungsraum.text = room.PlayerCount + " / " + 4;
            }
        }
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined to Lobby");
    }


    #endregion

    #region private Methods
    void CreateAndJoinRoom()
    {
        string randomRoomName = "Room_" + mapType + Random.Range(1, 100000);
        RoomOptions roomOptions = new RoomOptions();
        //roomOptions.isVisible()
        roomOptions.MaxPlayers = 4;

        string[] roomPropsInLobby = { MultiplayerVRConstants.MAP_TYPE_KEY };
        //two different maps: outdoor and school
        ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable() { { MultiplayerVRConstants.MAP_TYPE_KEY, mapType } };

        roomOptions.CustomRoomPropertiesForLobby = roomPropsInLobby;
        roomOptions.CustomRoomProperties = customRoomProperties;
        PhotonNetwork.CreateRoom(randomRoomName, roomOptions);
    }
    #endregion
}
