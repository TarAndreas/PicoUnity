using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

/// <summary>
/// This script assigns the player's local TeleportationProvider to all TeleportationArea components in the scene.
/// Intended for multiplayer XR (VR/AR) environments using Photon PUN.
/// Assumes TeleportationArea and TeleportationProvider are from Unity's XR Interaction Toolkit or similar system.
/// </summary>
public class PlayerAssign : MonoBehaviourPunCallbacks
{
    // Reference to the local player's XR Rig GameObject, expected to have a TeleportationProvider component.
    public GameObject LocalXRRigGameObject;

    void Start()
    {
        // Find all TeleportationArea components currently active in the scene.
        UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationArea[] teleportationAreas = GameObject.FindObjectsOfType<UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationArea>();

        if (teleportationAreas.Length > 0)
        {
            Debug.Log("Found " + teleportationAreas.Length + " teleportation areas.");
            // Loop through each found TeleportationArea.
            foreach (var item in teleportationAreas)
            {
                // Assign the TeleportationProvider from the local XR Rig to the TeleportationArea.
                // This ensures that teleportation actions in any area are handled by the local player's XR system.
                item.teleportationProvider = LocalXRRigGameObject.GetComponent<UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationProvider>();
            }
        }
    }
}
