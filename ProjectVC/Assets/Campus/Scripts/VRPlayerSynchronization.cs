using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/// <summary>
/// Synchronizes position and rotation data for a VR player avatar and both hands across the network using Photon PUN.
/// Ensures smooth interpolation, lag compensation, and initial snap for late joiners.
/// </summary>
public class VRPlayerSynchronization : MonoBehaviour, IPunObservable
{
    private PhotonView m_PhotonView; // Reference to PhotonView for ownership and RPCs

    // --------- Main VRPlayer Transform Synchronization -----------
    [Header("Main VRPlayer Transform Synch")]
    public Transform generalVRPlayerTransform; // The root of the VR player (typically XR Rig)

    // Position sync variables
    private float m_Distance_GeneralVRPlayer;
    private Vector3 m_Direction_GeneralVRPlayer;
    private Vector3 m_NetworkPosition_GeneralVRPlayer;
    private Vector3 m_StoredPosition_GeneralVRPlayer;

    // Rotation sync variables
    private Quaternion m_NetworkRotation_GeneralVRPlayer;
    private float m_Angle_GeneralVRPlayer;

    // --------- Hands Synchronization -----------
    [Header("Hands Transform Synch")]
    public Transform leftHandTransform;
    public Transform rightHandTransform;

    // Left hand position/rotation sync variables
    private float m_Distance_LeftHand;
    private Vector3 m_Direction_LeftHand;
    private Vector3 m_NetworkPosition_LeftHand;
    private Vector3 m_StoredPosition_LeftHand;
    private Quaternion m_NetworkRotation_LeftHand;
    private float m_Angle_LeftHand;

    // Right hand position/rotation sync variables
    private float m_Distance_RightHand;
    private Vector3 m_Direction_RightHand;
    private Vector3 m_NetworkPosition_RightHand;
    private Vector3 m_StoredPosition_RightHand;
    private Quaternion m_NetworkRotation_RightHand;
    private float m_Angle_RightHand;

    // Used to snap transforms for the initial update and avoid interpolation jumping
    bool m_firstTake = false;

    /// <summary>
    /// Initialize all network synchronization variables and get the PhotonView.
    /// </summary>
    public void Awake()
    {
        m_PhotonView = GetComponent<PhotonView>();

        // Store initial positions and set default networked data
        m_StoredPosition_GeneralVRPlayer = generalVRPlayerTransform.position;
        m_NetworkPosition_GeneralVRPlayer = Vector3.zero;
        m_NetworkRotation_GeneralVRPlayer = Quaternion.identity;

        m_StoredPosition_LeftHand = leftHandTransform.localPosition;
        m_NetworkPosition_LeftHand = Vector3.zero;
        m_NetworkRotation_LeftHand = Quaternion.identity;

        m_StoredPosition_RightHand = rightHandTransform.localPosition;
        m_NetworkPosition_RightHand = Vector3.zero;
        m_NetworkRotation_RightHand = Quaternion.identity;
    }

    /// <summary>
    /// Called when the script or GameObject becomes enabled.
    /// Sets flag to process an immediate snap on next network update.
    /// </summary>
    void OnEnable()
    {
        m_firstTake = true;
    }

    /// <summary>
    /// Every frame, if this isn't the local player, interpolate 
    /// avatar and hand transforms toward the latest networked states.
    /// </summary>
    public void Update()
    {
        if (!this.m_PhotonView.IsMine)
        {
            // Smoothly interpolate main avatar's position and rotation
            generalVRPlayerTransform.position = Vector3.MoveTowards(
                generalVRPlayerTransform.position,
                m_NetworkPosition_GeneralVRPlayer,
                m_Distance_GeneralVRPlayer * (1.0f / PhotonNetwork.SerializationRate));

            generalVRPlayerTransform.rotation = Quaternion.RotateTowards(
                generalVRPlayerTransform.rotation,
                m_NetworkRotation_GeneralVRPlayer,
                m_Angle_GeneralVRPlayer * (1.0f / PhotonNetwork.SerializationRate));

            // Smoothly interpolate left hand's position and rotation
            leftHandTransform.localPosition = Vector3.MoveTowards(
                leftHandTransform.localPosition,
                m_NetworkPosition_LeftHand,
                m_Distance_LeftHand * (1.0f / PhotonNetwork.SerializationRate));

            leftHandTransform.localRotation = Quaternion.RotateTowards(
                leftHandTransform.localRotation,
                m_NetworkRotation_LeftHand,
                m_Angle_LeftHand * (1.0f / PhotonNetwork.SerializationRate));

            // Smoothly interpolate right hand's position and rotation
            rightHandTransform.localPosition = Vector3.MoveTowards(
                rightHandTransform.localPosition,
                m_NetworkPosition_RightHand,
                m_Distance_RightHand * (1.0f / PhotonNetwork.SerializationRate));

            rightHandTransform.localRotation = Quaternion.RotateTowards(
                rightHandTransform.localRotation,
                m_NetworkRotation_RightHand,
                m_Angle_RightHand * (1.0f / PhotonNetwork.SerializationRate));
        }
    }

    /// <summary>
    /// Custom Photon serialization logic for all VR transforms of this player (avatar + both hands).
    /// Ensures all clients can reconstruct the correct state.
    /// </summary>
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //-------------------------------------------------------------
            // LOCAL OWNER: Sending own state

            // ---- Main avatar position/rotation ----
            m_Direction_GeneralVRPlayer = generalVRPlayerTransform.position - m_StoredPosition_GeneralVRPlayer;
            m_StoredPosition_GeneralVRPlayer = generalVRPlayerTransform.position;

            stream.SendNext(generalVRPlayerTransform.position);
            stream.SendNext(m_Direction_GeneralVRPlayer);
            stream.SendNext(generalVRPlayerTransform.rotation);

            // ---- Left hand ----
            m_Direction_LeftHand = leftHandTransform.localPosition - m_StoredPosition_LeftHand;
            m_StoredPosition_LeftHand = leftHandTransform.localPosition;

            stream.SendNext(leftHandTransform.localPosition);
            stream.SendNext(m_Direction_LeftHand);
            stream.SendNext(leftHandTransform.localRotation);

            // ---- Right hand ----
            m_Direction_RightHand = rightHandTransform.localPosition - m_StoredPosition_RightHand;
            m_StoredPosition_RightHand = rightHandTransform.localPosition;

            stream.SendNext(rightHandTransform.localPosition);
            stream.SendNext(m_Direction_RightHand);
            stream.SendNext(rightHandTransform.localRotation);
        }
        else
        {
            //-------------------------------------------------------------
            // REMOTE: Receiving other's state

            // ---- Main avatar position ----
            m_NetworkPosition_GeneralVRPlayer = (Vector3)stream.ReceiveNext();
            m_Direction_GeneralVRPlayer = (Vector3)stream.ReceiveNext();

            if (m_firstTake)
            {
                generalVRPlayerTransform.position = m_NetworkPosition_GeneralVRPlayer;
                m_Distance_GeneralVRPlayer = 0f;
            }
            else
            {
                // Lag compensation
                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
                m_NetworkPosition_GeneralVRPlayer += m_Direction_GeneralVRPlayer * lag;
                m_Distance_GeneralVRPlayer = Vector3.Distance(generalVRPlayerTransform.position, m_NetworkPosition_GeneralVRPlayer);
            }

            // ---- Main avatar rotation ----
            m_NetworkRotation_GeneralVRPlayer = (Quaternion)stream.ReceiveNext();
            if (m_firstTake)
            {
                m_Angle_GeneralVRPlayer = 0f;
                generalVRPlayerTransform.rotation = m_NetworkRotation_GeneralVRPlayer;
            }
            else
            {
                m_Angle_GeneralVRPlayer = Quaternion.Angle(generalVRPlayerTransform.rotation, m_NetworkRotation_GeneralVRPlayer);
            }

            // ---- Left hand position ----
            m_NetworkPosition_LeftHand = (Vector3)stream.ReceiveNext();
            m_Direction_LeftHand = (Vector3)stream.ReceiveNext();

            if (m_firstTake)
            {
                leftHandTransform.localPosition = m_NetworkPosition_LeftHand;
                m_Distance_LeftHand = 0f;
            }
            else
            {
                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
                m_NetworkPosition_LeftHand += m_Direction_LeftHand * lag;
                m_Distance_LeftHand = Vector3.Distance(leftHandTransform.localPosition, m_NetworkPosition_LeftHand);
            }

            // ---- Left hand rotation ----
            m_NetworkRotation_LeftHand = (Quaternion)stream.ReceiveNext();
            if (m_firstTake)
            {
                m_Angle_LeftHand = 0f;
                leftHandTransform.localRotation = m_NetworkRotation_LeftHand;
            }
            else
            {
                m_Angle_LeftHand = Quaternion.Angle(leftHandTransform.localRotation, m_NetworkRotation_LeftHand);
            }

            // ---- Right hand position ----
            m_NetworkPosition_RightHand = (Vector3)stream.ReceiveNext();
            m_Direction_RightHand = (Vector3)stream.ReceiveNext();

            if (m_firstTake)
            {
                rightHandTransform.localPosition = m_NetworkPosition_RightHand;
                m_Distance_RightHand = 0f;
            }
            else
            {
                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
                m_NetworkPosition_RightHand += m_Direction_RightHand * lag;
                m_Distance_RightHand = Vector3.Distance(rightHandTransform.localPosition, m_NetworkPosition_RightHand);
            }

            // ---- Right hand rotation ----
            m_NetworkRotation_RightHand = (Quaternion)stream.ReceiveNext();
            if (m_firstTake)
            {
                m_Angle_RightHand = 0f;
                rightHandTransform.localRotation = m_NetworkRotation_RightHand;
            }
            else
            {
                m_Angle_RightHand = Quaternion.Angle(rightHandTransform.localRotation, m_NetworkRotation_RightHand);
            }

            // Reset "first take" after processing the first update packet
            if (m_firstTake)
            {
                m_firstTake = false;
            }
        }
    }
}