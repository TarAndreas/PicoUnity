using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/// <summary>
/// Handles logic for a multiplayer snap zone (e.g., for assembling blocks/bricks in a construction/exercise context).
/// Tracks the number of correctly placed bricks, syncs audio feedback between players using Photon RPC calls,
/// and manages game progression based on the number of correct bricks.
/// </summary>
public class SnapZoneAction : MonoBehaviour
{
    // Reference to a collider GameObject (e.g., for enabling or disabling further interaction when task is complete)
    public GameObject colliderObject;

    // Audio clips for feedback (to be assigned in the Inspector)
    public AudioClip otherClip;   // Sound when construction is correct
    public AudioClip wrongClip;   // Sound when construction is incorrect

    // The required number of bricks for the task (set this in the Inspector)
    public int NumberOfBricks = 0;

    // Counter for how many correct bricks are currently in the snap zone.
    private int CorrectBricks = 0;

    /// <summary>
    /// Call this method when a brick enters the snap zone.
    /// Argument bCheck: true if the brick is correct, false otherwise.
    /// </summary>
    public void BrickOnSnapZone(bool bCheck)
    {
        if (bCheck == true)
        {
            // Increment counter for correct bricks.
            CorrectBricks += 1;
            Debug.Log("number of correct bricks: " + CorrectBricks);
        }
        else
        {
            // If brick is not correct, trigger the leave logic (decrements count if necessary).
            LeaveSnapZone();
        }
    }

    /// <summary>
    /// Call via RPC to reduce the correct brick count (e.g., when a brick is removed).
    /// The [PunRPC] attribute makes this callable by Photon across the network.
    /// </summary>
    [PunRPC]
    public void LeaveSnapZone()
    {
        if (CorrectBricks > 0)
        {
            CorrectBricks -= 1;
            Debug.Log("number of correct bricks: " + CorrectBricks);
        }
    }

    /// <summary>
    /// Public method to trigger an audio/feedback action.
    /// Executes logic via network so all players get consistent feedback.
    /// </summary>
    public void ExecuteAudioRpc()
    {
        PhotonView myPV = PhotonView.Get(this);
        myPV.RPC("ExecuteAction", RpcTarget.All);
    }

    /// <summary>
    /// Main feedback logic, called via network RPC.
    /// Plays success or error audio depending on whether task is complete.
    /// </summary>
    [PunRPC]
    public void ExecuteAction()
    {
        PhotonView myPV = PhotonView.Get(this);

        if (CorrectBricks == NumberOfBricks)
        {
            // If the goal is reached, deactivate the collider and play the correct sound.
            colliderObject.SetActive(false);
            AudioSource audio = GetComponent<AudioSource>();
            audio.clip = otherClip;
            audio.Play();
            Debug.Log("Playing audio for correct bricks");
        }
        else
        {
            // Task not complete: play error/negative feedback on all clients.
            myPV.RPC("PlayWrongAudio", RpcTarget.All);
        }
    }

    /// <summary>
    /// Plays the sound for an incorrect or incomplete construction.
    /// Called across all clients for feedback consistency.
    /// </summary>
    [PunRPC]
    public void PlayWrongAudio()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = wrongClip;
        audio.Play();
    }
}