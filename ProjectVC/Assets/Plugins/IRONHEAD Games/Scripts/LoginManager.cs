using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class LoginManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField PlayerName_Inputfield;
    #region UNITY Methods
    // Start is called before the first frame update
    void Start()
    {
       // PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    #region UI Callback Methods
    public void ConnectToPhotonServer()
    {
        if(PlayerName_Inputfield != null)
        {
            PhotonNetwork.NickName = PlayerName_Inputfield.text;
            PhotonNetwork.ConnectUsingSettings();
        }
        
    }
    #endregion

    #region Photon Callback Methods
    public override void OnConnected()
    {
        Debug.Log("OnConnected is called. The Server is available.");
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to the Master Server with Playername:" + PhotonNetwork.NickName);
        PhotonNetwork.LoadLevel("HomeScene");
    }
    
    #endregion
}
