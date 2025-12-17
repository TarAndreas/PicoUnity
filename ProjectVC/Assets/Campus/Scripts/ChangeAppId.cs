/*
 * Show the current AppID in the Photon settings
 * Change the AppID in the text box
 * Save the new AppID in the Photon settings
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class ChangeAppId : MonoBehaviour
{
    //public string appId;
    public TMP_InputField inputFieldAppId;
    private string appId;

    public void ChangePUNAppID()
    {
        appId = inputFieldAppId.text;
        PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = appId;
        PhotonNetwork.ConnectUsingSettings();
    }

    public void DisconnectFromCurrentSession()
    {
        PhotonNetwork.Disconnect();
    }
}
