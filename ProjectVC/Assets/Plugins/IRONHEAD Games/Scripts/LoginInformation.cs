using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using static SpawnManager;

public class LoginInformation : MonoBehaviourPunCallbacks
{

    public string adminName1 = "admin1";
    public string adminName2 = "admin2";
    public string adminName3 = "admin3";
    public string passwort = "master321";
    public TMP_InputField AdminName_Inputfield;
    public TMP_InputField Password_Inputfield;
    public bool bIsAdmin = false;
    private string nameType = "";
    private string passwordType = "";
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Pruefen ob der richtige Name eingetragen wurde
    public void checkName()
    {
        
        nameType = AdminName_Inputfield.text;
        Debug.Log("Derzeitiger Name " + nameType);

        if (nameType == adminName1)
        {
            checkPassword();
        } 
        else if (nameType == adminName2)
        {
            checkPassword();
        } 
        else if (nameType == adminName3)
        {
            checkPassword();
        }
        else
        {
            Debug.Log("Falscher Name");
            
        }
    }

    // Pruefen ob das richtige Passwort eingegeben wurde
    public void checkPassword()
    {
        passwordType = Password_Inputfield.text;

        if (passwordType == passwort)
        {
            connectToServerAsAdmin();
            SpawnManager.isAdmin(true);
        }
        else
        {
            Debug.Log("Falsches Passwort");
            SpawnManager.isAdmin(false);
            
        }
    }

    //Mit dem Photon Netzwerk verbinden
    public void connectToServerAsAdmin()
    {
        PhotonNetwork.NickName = AdminName_Inputfield.text;
        PhotonNetwork.ConnectUsingSettings();
        //PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
    }

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
