using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

using TMPro;

/// <summary>
/// This script configures the player's XR rig and interaction capabilities at session start,
/// setting up teleportation and displaying the player's name. 
/// Designed for Photon PUN multiplayer scenarios with Unity's XR Interaction Toolkit.
/// </summary>
public class PlayerSessionSetup : MonoBehaviourPunCallbacks
{
    // Reference to the local player's XR rig (controllers, camera, etc.), set in the Inspector.
    public GameObject LocalXRRigGameObject;

    // Reference to a TextMeshProUGUI element (in the scene UI) for showing the player's name.
    public TextMeshProUGUI PlayerName_Text;

    // Start() is called before the first frame update by Unity.
    void Start()
    {
        // Only do local setup for the player object owned by this client.
        if (photonView.IsMine)
        {
            // Enable this player's XR rig for local control and interaction.
            LocalXRRigGameObject.SetActive(true);

            // Find all teleportation areas in the scene and assign this player's teleportation provider.
            UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationArea[] teleportationAreas = GameObject.FindObjectsOfType<UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationArea>();
            if (teleportationAreas.Length > 0)
            {
                Debug.Log("Found " + teleportationAreas.Length + " teleportation areas.");
                foreach (var item in teleportationAreas)
                {
                    // Ensure that teleportation actions in these areas use this local player's logic.
                    item.teleportationProvider = LocalXRRigGameObject.GetComponent<UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationProvider>();
                }
            }

            // Repeat the process for teleportation anchors (fixed discrete points).
            UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationAnchor[] teleportationAnchors = GameObject.FindObjectsOfType<UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationAnchor>();
            if (teleportationAnchors.Length > 0)
            {
                Debug.Log("Found " + teleportationAnchors.Length + " teleportation areas.");
                foreach (var item in teleportationAnchors)
                {
                    item.teleportationProvider = LocalXRRigGameObject.GetComponent<UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationProvider>();
                }
            }

            // Attach an AudioListener for spatial audio; only the local player should have this enabled.
            LocalXRRigGameObject.AddComponent<AudioListener>();
        }
        else
        {
            // For remote players, disable their rig on this client to avoid control conflicts and performance waste.
            LocalXRRigGameObject.SetActive(false);
        }

        // Update UI: If the player name text is set, display the current Photon network nickname.
        if (PlayerName_Text.text != null)
        {
            PlayerName_Text.text = photonView.Owner.NickName;
        }
    }
}
