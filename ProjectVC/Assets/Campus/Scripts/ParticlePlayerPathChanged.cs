using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/// <summary>
/// This script synchronizes the activation and deactivation of a ParticleSystem
/// on a networked object using Photon PUN. When a player enters or exits a collider zone,
/// it enables or disables particle effects for all clients.
/// </summary>
public class ParticlePlayerPathChanged : MonoBehaviour
{
    // Reference to the GameObject containing the ParticleSystem to be controlled.
    public GameObject observedItem;

    // Tracks whether the observed object (e.g., player) is currently inside the defined trigger zone.
    public bool isInZone = false;

    // Cached reference to the ParticleSystem component.
    private ParticleSystem particles;

    // Start is called before the first frame update, used here for component initialization.
    void Start()
    {
        // Retrieves the first ParticleSystem found in the observedItem's children.
        particles = observedItem.GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame.
    // Responsible for triggering particle activation via networked RPC if inside the zone.
    void Update()
    {
        PhotonView myPV = PhotonView.Get(this);

        // If the object is in the trigger zone, enable the particle effect for all clients.
        if (isInZone == true)
        {
            myPV.RPC("enablePlayerPath", RpcTarget.All);
        }
    }

    // Called when another collider enters this object's trigger collider.
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered");
        isInZone = true;
    }

    // Called when another collider exits this object's trigger collider.
    public void OnTriggerExit(Collider collision)
    {
        PhotonView mpv = PhotonView.Get(this);

        Debug.Log("Trigger left");
        isInZone = false;
        // Optionally, could call disablePlayerPath() locally.
        mpv.RPC("disablePlayerPath", RpcTarget.All);
    }

    /// <summary>
    /// Called via Photon RPC. Plays (starts or resumes) the ParticleSystem on all clients.
    /// Ensures all players see the same visual change.
    /// </summary>
    [PunRPC]
    public void enablePlayerPath()
    {
        particles.Play();
    }

    /// <summary>
    /// Called via Photon RPC. Pauses the ParticleSystem on all clients.
    /// Use Pause to retain the current visual state rather than resetting.
    /// </summary>
    [PunRPC]
    public void disablePlayerPath()
    {
        particles.Pause();
    }

    /// <summary>
    /// Public method to request full stop of the ParticleSystem network-wide.
    /// Can be called by UI or other scripts to fully remove/halt the effect.
    /// </summary>
    public void removeParticles()
    {
        PhotonView pv = PhotonView.Get(this);
        pv.RPC("stopParticles", RpcTarget.All);
    }

    /// <summary>
    /// Called via Photon RPC. Fully stops (resets and disables) the ParticleSystem on all clients.
    /// </summary>
    [PunRPC]
    public void stopParticles()
    {
        particles.Stop();
    }
}