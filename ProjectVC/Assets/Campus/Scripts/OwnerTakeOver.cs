using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// Handles network ownership transfer of a PhotonView, e.g., when interacting with objects in a multiplayer setting.
/// Can be used for VR/AR object pickup and handover scenarios.
/// </summary>
public class OwnerTakeOver : MonoBehaviourPun, IPunOwnershipCallbacks
{
    // Reference to the PhotonView attached to this GameObject
    PhotonView m_photonView;

    /// <summary>
    /// Initialize m_photonView reference.
    /// </summary>
    private void Awake()
    {
        m_photonView = GetComponent<PhotonView>();
    }

    /// <summary>
    /// Call this when the local player wants to take/select this object.
    /// Typically triggered by a user input event (e.g., "pick up" action).
    /// </summary>
    public void OnSelectEnter()
    {

        Debug.Log("OnSelectEnter()");

        // If the local player already owns this object, do nothing
        if (m_photonView.Owner == PhotonNetwork.LocalPlayer)
        {
            Debug.Log("Not requesting ownership. Already mine.");
        }
        else
        {
            // Otherwise, initiate an ownership transfer request to become the owner
            TransferOwnership();
        }

    }

    /// <summary>
    /// Requests ownership of the object via Photon networking.
    /// Notifies the current owner and delegates approval as per Photon setup.
    /// </summary>
    void TransferOwnership()
    {
        Debug.Log("TransferOwnership()");
        m_photonView.RequestOwnership();
    }

    /// <summary>
    /// Callback received when another player requests ownership of this object.
    /// Approves the request by explicitly transferring ownership.
    /// </summary>
    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        // Only respond if the request is for this PhotonView
        if (targetView != m_photonView)
        {
            return;
        }
        Debug.Log("OnOwnershipRequest()");
        Debug.Log("OnOwnerShip Requested for: " + targetView.name + " from " + requestingPlayer.NickName);

        // Accept the request and transfer ownership
        m_photonView.TransferOwnership(requestingPlayer);
    }

    /// <summary>
    /// Callback when an ownership transfer has been completed.
    /// Allows for feedback or logic upon change of network authority.
    /// </summary>
    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        Debug.Log("OnOwnershipTransfered()");
        // You might add logic here to notify users, update UI, or reset state, etc.
    }

    /// <summary>
    /// Callback when an attempted ownership transfer fails.
    /// Use for user feedback or error handling.
    /// </summary>
    public void OnOwnershipTransferFailed(PhotonView previousView, Player previousPlayer)
    {
        Debug.Log("OnOwnershipTransferFailed");
        // Here you could implement retries, user notifications, etc.
    }

}
