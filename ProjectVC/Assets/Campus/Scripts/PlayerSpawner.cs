using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/// <summary>
/// Responsible for spawning the local VR player avatar at a specific position when the game starts,
/// ensuring that networking is set up before instantiation (Photon PUN).
/// </summary>
public class PlayerSpawner : MonoBehaviour
{
    // The player prefab to instantiate in the networked scene. Assign this in the Unity inspector.
    [SerializeField]
    GameObject VRPlayerPrefab;

    // The position at which to spawn the player object.
    public Vector3 spawnPosition;

    /// <summary>
    /// Called automatically by Unity when the object becomes active.
    /// Handles connection to the Photon network (if necessary) and player instantiation.
    /// </summary>
    void Start()
    {
        Debug.Log("In the start function");

        // Check if the client is already connected and ready to use Photon PUN services.
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            // If we're not connected, connect using the project's Photon settings.
            PhotonNetwork.ConnectUsingSettings();
            Debug.Log("Connect to the Network");
        }
        else
        {
            // If already connected, instantiate the player prefab at the specified position
            // and with no rotation. Instantiation is networked (visible to all clients).
            PhotonNetwork.Instantiate(VRPlayerPrefab.name, spawnPosition, Quaternion.identity);
            Debug.Log("Instantiate Player");
        }
    }
}