using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChangeRoom : MonoBehaviour
{

    public GameObject buttonChange;
    private GameObject[] admins;

    //Set menu for user active if Admin VR player is used

    void Start()
    {
        buttonChange.SetActive(false);
        admins = GameObject.FindGameObjectsWithTag("Admin");

        //PhotonNetwork.IsMasterClient

        if (admins != null)
        {
            buttonChange.SetActive(true);
        }

    }

    //Load scene Outdoor

    public void loadLevelOutdoor()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("World_Outdoor");
            PhotonNetwork.AutomaticallySyncScene = true;
        }
    }

    //Load scene test

    public void loadLevelLabyrinth()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("test");
            PhotonNetwork.AutomaticallySyncScene = true;
        }
    }

    //Load scene School

    public void loadLevelSchool()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("World_School");
            PhotonNetwork.AutomaticallySyncScene = true;
        }
    }



    //Load scene Besprechungsraum

    public void loadLevelBSRaum()
    {
        if (PhotonNetwork.IsMasterClient)
        {
        PhotonNetwork.LoadLevel("Besprechungsraum");
        PhotonNetwork.AutomaticallySyncScene = true;
        }
    }

}
