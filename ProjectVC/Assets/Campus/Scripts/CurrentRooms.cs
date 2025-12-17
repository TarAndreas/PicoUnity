using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CurrentRooms : MonoBehaviour
{

    //public Text rName;
    public TMP_Text tmpName;
    public string tmpPass;
    public TMP_Text tmpPlayerNumbers;

    public void JoinRoom() {

        //GameObject.Find("RoomLoader").GetComponent<RoomLoader>().UseSettingsToConnect();
        //GameObject.Find("RoomLoader").GetComponent<RoomLoader>().OnConnectedToMaster();
        GameObject.Find("RoomLoader").GetComponent<RoomLoader>().JoinRoomInList(tmpPass);
        //GameObject.Find("RoomLoader").GetComponent<RoomLoader>().LoadLevelInList(tmpName.text);
    }

}
