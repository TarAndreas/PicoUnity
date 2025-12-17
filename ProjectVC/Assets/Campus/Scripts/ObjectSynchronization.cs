using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/// <summary>
/// Synchronizes the position and rotation of a Transform across the network using Photon PUN.
/// Implements IPunObservable for custom network serialization.
/// Attach this script to any GameObject you want to synchronize in a networked scene.
/// </summary>
public class ObjectSynchronization : MonoBehaviour, IPunObservable
{
    private PhotonView m_PhotonView; // Reference to the PhotonView component used for network ownership checks

    
    [Header("Object Transform Synchronization")]
    public Transform objectTransform; // The transform you wish to synchronize (usually self or a child)

    
    private float m_Distance_Object; // The distance between the current and network position
    private Vector3 m_Direction_Object; // Direction vector sent for interpolation and lag compensation
    private Vector3 m_NetworkPosition_Object; // The latest position received from the network
    private Vector3 m_StoredPosition_Object; // Last position locally stored/sent

    
    private Quaternion m_NetworkRotation_Object; // The latest rotation received from the network
    private float m_Angle_Object; // Angle difference between current and network rotation

    
    bool m_firstTake = false; // Used to apply the first update immediately and avoid large jumps

    /// <summary>
    /// Initialize PhotonView reference and synchronization variables.
    /// </summary>
    public void Awake()
    {
        m_PhotonView = GetComponent<PhotonView>();

        // Store the initial position and reset network position/rotation
        m_StoredPosition_Object = objectTransform.position;
        m_NetworkPosition_Object = Vector3.zero;
        m_NetworkRotation_Object = Quaternion.identity;
    }

    /// <summary>
    /// Called when the object is enabled. Used to trigger immediate sync on first network update.
    /// </summary>
    void OnEnable()
    {
        m_firstTake = true;
    }

    /// <summary>
    /// Update runs every frame. Handles smooth interpolation of position and rotation
    /// towards the values received from the network.
    /// </summary>
    void Update()
    {
        // Smoothly move towards the target position using received data
        objectTransform.position = Vector3.MoveTowards(
            objectTransform.position,
            this.m_NetworkPosition_Object,
            this.m_Distance_Object * (1.0f / PhotonNetwork.SerializationRate)
        );

        // Smoothly rotate towards the target rotation using received data
        objectTransform.rotation = Quaternion.RotateTowards(
            objectTransform.rotation,
            this.m_NetworkRotation_Object,
            this.m_Angle_Object * (1.0f / PhotonNetwork.SerializationRate)
        );
    }

    /// <summary>
    /// Called by Photon to serialize and deserialize the object's transform data.
    /// Handles both writing (sending) and reading (receiving) states.
    /// </summary>
    /// <param name="stream">Photon stream; use for sending or receiving data</param>
    /// <param name="info">Information about the message (including lag)</param>
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) // We own this object: send data to others
        {
            // Compute the direction since last frame (helps with lag compensation)
            this.m_Direction_Object = objectTransform.position - this.m_StoredPosition_Object;
            this.m_StoredPosition_Object = objectTransform.position;

            // Send position and direction
            stream.SendNext(objectTransform.position);
            stream.SendNext(this.m_Direction_Object);

            // Send rotation as well
            stream.SendNext(objectTransform.rotation);
        }
        else 
        {
            // Receive position and direction
            this.m_NetworkPosition_Object = (Vector3)stream.ReceiveNext();
            this.m_Direction_Object = (Vector3)stream.ReceiveNext();

            if (m_firstTake)
            {
                // On the first update, snap to the correct position
                objectTransform.position = this.m_NetworkPosition_Object;
                this.m_Distance_Object = 0f;
            }
            else
            {
                // Lag compensation: account for network delay so we can predict "ahead"
                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
                this.m_NetworkPosition_Object += this.m_Direction_Object * lag;
                this.m_Distance_Object = Vector3.Distance(objectTransform.position, this.m_NetworkPosition_Object);
            }

            // Receive rotation
            this.m_NetworkRotation_Object = (Quaternion)stream.ReceiveNext();
            if (m_firstTake)
            {
                // On the first update, snap rotation
                this.m_Angle_Object = 0f;
                objectTransform.rotation = this.m_NetworkRotation_Object;
            }
            else
            {
                // Compute angle difference for smooth interpolation
                this.m_Angle_Object = Quaternion.Angle(objectTransform.rotation, this.m_NetworkRotation_Object);
            }

            // Clear first-take flag after first update is processed
            if (m_firstTake)
            {
                m_firstTake = false;
            }
        }
    }
}