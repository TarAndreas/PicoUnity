using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class QuitOption : MonoBehaviour
{

    public GameObject button;
    

    void Start()
    {
        button.SetActive(false);

        if (PhotonNetwork.IsMasterClient)
        {
            button.SetActive(true);
        }
        
    }

    public void doQuit()
    {
        Debug.Log ("Projekt wird beendet");
            Application.Quit();
    }


}
