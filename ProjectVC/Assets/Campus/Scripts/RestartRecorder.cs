using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Voice.Unity;
using Photon.Realtime;

/// <summary>
/// This script monitors the number of players in the current Photon room.
/// If the player count increases, it restarts the Photon Voice Recorder after a short delay.
/// This helps ensure the recorder is properly synchronized or refreshed in dynamic multiplayer situations.
/// </summary>
public class RestartRecorder : MonoBehaviour
{

    private byte i = 0;

    // Reference to the Photon Voice Recorder component for this user/scene.
    public Recorder recorder;

    
    void Update()
    {
        // Detect if the player count in the Photon room has changed since last check.
        if (PhotonNetwork.CurrentRoom.PlayerCount != i)
        {
            // 'i' is incremented to reflect the new number of players handled.
            // (Note: This only increments up, so it won’t respond to a player leaving.)
            i++;

            // Start the recorder restart process via coroutine (5 seconds delay).
            StartCoroutine(RecorderRestart());

            // For debugging: logs the current number of players in the room.
            Debug.Log("Current Players = " + PhotonNetwork.CurrentRoom.PlayerCount);
        }
    }

    /// <summary>
    /// Coroutine to restart the voice recorder after a delay.
    /// Used to let network/player joins stabilize before restarting voice services.
    /// </summary>
    IEnumerator RecorderRestart()
    {
        yield return new WaitForSeconds(5.0f); // Wait five seconds
        recorder.RestartRecording(true);       // Restart the voice recorder (with full reset)
    }

    /// <summary>
    /// Public utility function to manually restart the recorder immediately.
    /// Can be called, for example, from a UI button or other event.
    /// </summary>
    public void RestartSceneRecorder()
    {
        recorder.RestartRecording(true);
        Debug.Log("Restarting Recorder");
    }
}
