using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;



public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    GameObject GenericVRPlayerPrefab;
    [SerializeField]
    GameObject GenericVRAdminPrefab;

    public Vector3 spawnPosition;
    private static bool bReturnAdmin = false;
    public bool isMaster = false;

    // Start is called before the first frame update
    void Start()
    {
        if(PhotonNetwork.IsConnectedAndReady)
        {
            //PhotonNetwork.Instantiate(GenericVRPlayerPrefab.name, spawnPosition, Quaternion.identity );
            InstantiateVRPlayer();

        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static bool isAdmin(bool bCheckName)
    {
        return bReturnAdmin = bCheckName;
        
    }

    // Load Player Rig depending on its role. If Admin make it Master Client.

    public void InstantiateVRPlayer()
    {
        
        

        if (bReturnAdmin == true)
        {
            PhotonNetwork.Instantiate(GenericVRAdminPrefab.name, spawnPosition, Quaternion.identity);
            PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
            isMaster = true;

        } else
        {
            
            PhotonNetwork.Instantiate(GenericVRPlayerPrefab.name, spawnPosition, Quaternion.identity);
        }
        
    }
}
