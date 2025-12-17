using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UIElements;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Serializable class to define the default settings for a room,
/// including its display name, scene index for loading, and maximum players.
/// </summary>
[System.Serializable]
public class DefaultRoom
{
    public string Name;
    public int sceneIndex;
    public int maxPlayer;
}

/// <summary>
/// This class manages Photon room/lobby connection, room creation/joining,
/// UI state when connecting, as well as scene loading in a networked VR context.
/// It also allows for password-protected rooms and manages UI logic on Photon events.
/// </summary>
public class RoomLoader : MonoBehaviourPunCallbacks
{
    // Scene index used for scene loading, initially set to 10 (interpreted as 'unset')
    private int SceneIndexNumber = 10;

    // List of preconfigured room options, set in the inspector
    public List<DefaultRoom> defaultRooms;

    // If true, the client will automatically attempt to join a room after lobby join
    public bool autoJoinRoom;

    // UI: GameObjects to enable/disable upon Photon connection
    public List<GameObject> objectsToEnableOnPhotonConnect;
    public List<GameObject> objectsToDisableOnPhotonConnect;

    // UI: Toggle and input field for optional password protection on rooms
    public UnityEngine.UI.Toggle toggleMitPass;
    public TMP_InputField passwordInputField;

    // Internal: state for password-protection
    private bool mitPass;        // Tracks toggle state (show/hide password UI logic)
    private string passWordRom;  // Stores the user-set room password

    private void Update()
    {
        // Update password toggle state each frame, so UI can react correspondingly
        mitPass = toggleMitPass.isOn;

        // You could use these lines to show/hide password input field dynamically
        // passwordInputField.gameObject.SetActive(toggleMitPass.isOn);
    }

    void Start()
    {
        // Ensure all clients load the same scene when the master loads one
        PhotonNetwork.AutomaticallySyncScene = true;

        // Connect this client to the Photon Master server using project settings
        PhotonNetwork.ConnectUsingSettings();
        //PhotonNetwork.PhotonServerSettings.AppSettings;
    }

    // Photon callback: called if joining a random room fails
    public override void OnJoinRandomFailed(short returncode, string message)
    {
        base.OnJoinRandomFailed(returncode, message);
        Debug.Log(message);
    }

    // Photon callback: called if joining a specific room fails
    public override void OnJoinRoomFailed(short returncode, string message)
    {
        base.OnJoinRoomFailed(returncode, message);
        Debug.Log(message);
    }

    // Photon callback: called if creating a room succeeded
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.Log("A room is created with name: " + PhotonNetwork.CurrentRoom.Name);
    }

    // Photon callback: after the client connects to the Photon master server
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Connected to Lobby again");

        // Enable or disable specific UI GameObjects when connected
        foreach (GameObject obj in objectsToEnableOnPhotonConnect)
        {
            obj.SetActive(true);
        }
        foreach (GameObject obj in objectsToDisableOnPhotonConnect)
        {
            obj.SetActive(false);
        }

        // Join the default Photon lobby (required to see rooms/listings)
        PhotonNetwork.JoinLobby();
        Debug.Log("Joined a Lobby");
    }

    // Photon callback: called if room creation fails (e.g., name clash)
    public override void OnCreateRoomFailed(short returncode, string message)
    {
        base.OnCreateRoomFailed(returncode, message);
        Debug.Log("Failed to create a room: " + message, this);
    }

    // Photon callback: called when another player enters the joined room
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnJoinedRoom();
        Debug.Log(newPlayer.NickName + " joined to:" + "Player count " + PhotonNetwork.CurrentRoom.PlayerCount);
        base.OnPlayerEnteredRoom(newPlayer);
    }

    // Photon callback: called when lobby join succeeds
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("Joined a Lobby");

        // Optionally, auto-join a specified room upon lobby join
        if (autoJoinRoom)
            InitializeRoom(1); // Uses second room in defaultRooms list (list is zero-indexed)
    }

    // Photon callback: called when client leaves the lobby
    public override void OnLeftLobby()
    {
        base.OnLeftLobby();
        Debug.Log("Left the Lobby");
    }

    // Photon callback: called when the client joins a room
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined a Room: " + PhotonNetwork.CurrentRoom.Name);
        LoadSelectedScene();
    }

    /// <summary>
    /// Configures and creates a room using settings from 'defaultRooms' list.
    /// Supports password-protected rooms if toggleMitPass is on and a password is entered.
    /// </summary>
    public void InitializeRoom(int defaultRoomIndex)
    {
        RoomOptions roomOptions = new RoomOptions();

        // If using password, require non-empty password
        if (toggleMitPass.isOn)
        {
            if (passwordInputField.text.Length < 1)
            {
                return; // Exit early if password is empty
            }
            passWordRom = passwordInputField.text;
            // roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable();
            // roomOptions.CustomRoomProperties.Add("password", passWordRom); // For advanced: add password as a custom room property
        }

        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];
        // For advanced: You might randomize or alter the name here for multiple similar rooms

        // Append password to name if required - NOTE: this is a basic approach!
        if (passWordRom != null)
        {
            roomSettings.Name = roomSettings.Name + passWordRom;
            passwordInputField.text = roomSettings.Name;
        }
        roomOptions.MaxPlayers = (byte)roomSettings.maxPlayer;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

        // Create or join a room with these settings
        PhotonNetwork.JoinOrCreateRoom(roomSettings.Name, roomOptions, TypedLobby.Default);
        Debug.Log("Creating a room: " + roomSettings.Name);

        // Store for later use when loading scene
        SceneIndexNumber = roomSettings.sceneIndex;
    }

    /// <summary>
    /// Loads the scene corresponding to last selected room,
    /// but only if a valid index has been selected.
    /// </summary>
    public void LoadSelectedScene()
    {
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

    /// <summary>
    /// Joins a room based on its name, checking for password if appropriate.
    /// </summary>
    public void JoinRoomInList(string RoomName)
    {
        // Ensure we are connected before attempting to join
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            UseSettingsToConnect();
            Debug.Log("Connected and ready");
        }

        // For special rooms (not public), check the password
        if (RoomName != "Garden" && RoomName != "Labor" && RoomName != "Lehramt")
        {
            if (RoomName == passwordInputField.text)
            {
                RoomName = passwordInputField.text;
                PhotonNetwork.JoinRoom(RoomName);
            }
            else
            {
                return;
            }
        }
        else
        {
            PhotonNetwork.JoinRoom(RoomName);
            Debug.Log("Joining Room in List");
        }
    }

    /// <summary>
    /// Connects using Photon project settings (helper for UI button).
    /// </summary>
    public void UseSettingsToConnect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    /// <summary>
    /// Loads a specific scene based on a room/level name.
    /// Used to allow users to jump to example scenes (by room type).
    /// </summary>
    public void LoadLevelInList(string LevelName)
    {
        if (LevelName.Contains("Garden"))
        {
            PhotonNetwork.LoadLevel(2);
            Debug.Log("Loading Level in List");
        }
        else if (LevelName.Contains("Labor"))
        {
            PhotonNetwork.LoadLevel(1);
            Debug.Log("Loading Level in List");
        }
        else if (LevelName.Contains("Lehramt"))
        {
            PhotonNetwork.LoadLevel(3);
            Debug.Log("Loading Level in List");
        }
        else
        {
            Debug.Log("Room name unequal to any Level name");
        }
    }
}