using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CustomTest : MonoBehaviour
{

    [SerializeField]
    GameObject VRPlayerPrefab;

    [SerializeField]
    GameObject VRPlayerPrefabLocal;

    public Vector3 spawnPosition;
    public Vector3 spawnPosition2;


    // Start is called before the first frame update
    void Start()
    {

        if (!PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.ConnectUsingSettings();
            Debug.Log("Connect to the Network");

        } else if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(this.VRPlayerPrefab.name, spawnPosition, Quaternion.identity);
            //transform.rotation
            Debug.Log("Instanting Player");
        } else
        {
            PhotonNetwork.Instantiate(this.VRPlayerPrefabLocal.name, spawnPosition2, Quaternion.identity);
        }

    }

}
