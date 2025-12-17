using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// Displays the list of available multiplayer rooms in the UI.
/// Instantiates a prefab for each room and populates its fields with room info,
/// grouping certain rooms by custom logic (name composition).
/// Should be attached to a manager object that exists in the room list scene/lobby.
/// </summary>
public class RoomList : MonoBehaviourPunCallbacks
{
    // Prefab for representing a room entry in the UI; assign in Inspector
    public GameObject RoomListPrefab;

    /// <summary>
    /// This callback is automatically called by Photon whenever the room list is updated.
    /// The argument contains the current list of available rooms on the master server.
    /// </summary>
    /// <param name="roomList">List of current rooms known to the Photon lobby server</param>
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            // Print the room name for debugging or logging
            print(roomList[i].Name);

            // Instantiate a new UI element for the room. 
            // The parent transform is set to a UI Canvas (must have this object in the scene).
            GameObject Rooms = Instantiate(RoomListPrefab, GameObject.Find("UI_Main_Canvas_Rooms").transform);

            // If not the first room, adjust y-position for vertical spacing.
            if (i > 0)
            {
                float yOffset = -(i - 0.67f); // Custom vertical offset; tweak as needed for UI spacing.
                Vector3 newPosition = Rooms.transform.position + new Vector3(0f, yOffset, 0f);
                Rooms.transform.position = newPosition;
            }

            // Display logic: by default, show the raw room name for certain "public" rooms. 
            // For others, show a grouped/abstracted name in the UI for privacy or simplicity.
            if (roomList[i].Name != "Garden" && roomList[i].Name != "Labor" && roomList[i].Name != "Lehramt")
            {
                // If it's a customized/private room, map it to a general public label if it contains keywords, otherwise "privat".
                if (roomList[i].Name.Contains("Garden"))
                {
                    Rooms.GetComponent<CurrentRooms>().tmpName.text = "Garden";
                    Rooms.GetComponent<CurrentRooms>().tmpPass = roomList[i].Name;
                }
                else if (roomList[i].Name.Contains("Labor"))
                {
                    Rooms.GetComponent<CurrentRooms>().tmpName.text = "Labor";
                    Rooms.GetComponent<CurrentRooms>().tmpPass = roomList[i].Name;
                }
                else if (roomList[i].Name.Contains("Lehramt"))
                {
                    Rooms.GetComponent<CurrentRooms>().tmpName.text = "Lehramt";
                    Rooms.GetComponent<CurrentRooms>().tmpPass = roomList[i].Name;
                }
                else
                {
                    // For all other cases, show as a "privat" (private) room label
                    Rooms.GetComponent<CurrentRooms>().tmpName.text = "privat";
                    Rooms.GetComponent<CurrentRooms>().tmpPass = roomList[i].Name;
                }

                // Show the number of players currently in the room
                Rooms.GetComponent<CurrentRooms>().tmpPlayerNumbers.text = roomList[i].PlayerCount.ToString();
            }
            else
            {
                // For public rooms with explicit names, display name and player count directly
                Rooms.GetComponent<CurrentRooms>().tmpPass = roomList[i].Name;
                Rooms.GetComponent<CurrentRooms>().tmpName.text = roomList[i].Name;
                Rooms.GetComponent<CurrentRooms>().tmpPlayerNumbers.text = roomList[i].PlayerCount.ToString();
            }
        }
    }
}