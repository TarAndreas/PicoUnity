using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/// <summary>
/// Spawns and synchronizes glowing orb particle effects at the position of a player,
/// triggered by entering and exiting a zone, using Photon RPCs for multiplayer consistency.
/// </summary>
public class ParticlePlayerPath : MonoBehaviour
{
    // Reference to the orb prefab to instantiate
    public GameObject glowOrb;

    // Reference to the player transform; orbs are spawned at this player's position
    public Transform player;

    // Whether an orb is currently being placed (controls coroutine state)
    private bool placingOrb = false;

    // Is the player currently inside the trigger zone?
    private bool isInZone = false;

    // List keeping track of all spawned orb GameObjects for later destruction
    private List<GameObject> orbClones = new List<GameObject>();

    /// <summary>
    /// Unity's update loop. Checks if player has entered the zone and if orbs are not already being placed.
    /// Starts the orb placement coroutine and synchronizes over network using a Photon RPC.
    /// </summary>
    void Update()
    {
        // Get the PhotonView associated with this script (required for making RPC calls)
        PhotonView myPV = PhotonView.Get(this);

        // When the player is in the zone and not already placing orbs, start placing orbs (start coroutine)
        if (isInZone == true && placingOrb == false)
        {
            placingOrb = true;
            // Use RPC to start coroutine on all clients for synchrony
            myPV.RPC("RpcStartCo", RpcTarget.All);
            // Optionally start locally: StartCoroutine(DropOrb()); // replaced by networked call
            player = player.transform; // redundant, as player is already a Transform
        };
    }

    /// <summary>
    /// Unity callback: called when a collider enters this object's trigger collider.
    /// Marks player as 'in zone'.
    /// </summary>
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered");
        isInZone = true;
    }

    /// <summary>
    /// Unity callback: called when a collider exits this object's trigger collider.
    /// Marks player as 'not in zone', then stops orb placement coroutine and destroys current orbs across all clients.
    /// </summary>
    public void OnTriggerExit(Collider other)
    {
        PhotonView pv = PhotonView.Get(this);

        Debug.Log("Trigger exited");
        isInZone = false;

        // Stop orb placement coroutine across the room
        pv.RPC("RpcStopCo", RpcTarget.All);
        // Destroy current orb clones across the room
        pv.RPC("DestroyClones", RpcTarget.All);
    }

    /// <summary>
    /// Coroutine that spawns an orb above the player, waits, and then allows for another orb to be spawned.
    /// Runs as a networked routine using RPC.
    /// </summary>
    [PunRPC]
    IEnumerator DropOrb()
    {
        // Instantiate a new orb a little above the player's position, rotated appropriately
        var orbClone = Instantiate(
            glowOrb,
            new Vector3(player.localPosition.x, player.localPosition.y + 0.1f, player.localPosition.z),
            Quaternion.Euler(90, 0, 0));
        orbClones.Add(orbClone);

        // Wait just under one second before allowing next orb to be placed
        yield return new WaitForSeconds(0.99f);
        placingOrb = false;
    }

    /// <summary>
    /// Destroys all orb clones on this client after a long delay (15 minutes).
    /// Called as a network RPC to ensure consistency across all clients.
    /// </summary>
    [PunRPC]
    public void DestroyClones()
    {
        foreach (var cloneObj in orbClones)
        {
            Destroy(cloneObj, 900.0f); // Delayed destruction (900 seconds = 15 minutes)
        }
    }

    /// <summary>
    /// Starts the DropOrb coroutine on this client; called as a network RPC.
    /// Ensures all clients start the effect in synchronicity.
    /// </summary>
    [PunRPC]
    public void RpcStartCo()
    {
        StartCoroutine(DropOrb());
    }

    /// <summary>
    /// Stops the DropOrb coroutine on this client; called as a network RPC.
    /// Stops ongoing orb spawning when player leaves the zone.
    /// </summary>
    [PunRPC]
    public void RpcStopCo()
    {
        StopCoroutine(DropOrb());
    }

    /// <summary>
    /// Begins the process of destroying orb clones on all clients with a short delay.
    /// Locally sends the network RPC that triggers `DestroyClonesNow` everywhere.
    /// </summary>
    public void RpcDestroyClones()
    {
        PhotonView pv = PhotonView.Get(this);
        pv.RPC("DestroyClonesNow", RpcTarget.All);
    }

    /// <summary>
    /// Destroys all orb clones on this client after a short delay (1 second).
    /// Called via network RPC for responsive feedback (e.g. immediate cleanup).
    /// </summary>
    [PunRPC]
    public void DestroyClonesNow()
    {
        foreach (var cloneObj in orbClones)
        {
            Destroy(cloneObj, 1.0f); // Delayed destruction (1 second)
        }
    }
}