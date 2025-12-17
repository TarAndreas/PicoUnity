using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script plays an audio effect attached to the same GameObject.
/// Typical use: Call PlayAudio() in response to a particle effect event 
/// or other gameplay trigger.
/// </summary>
public class ParticleSoundEffect : MonoBehaviour
{
    // Reference to the AudioSource component on this GameObject.
    private AudioSource audiosource;

    /// <summary>
    /// Plays the audio clip attached to the AudioSource.
    /// The AudioSource component must be attached to this GameObject in advance.
    /// </summary>
    public void PlayAudio()
    {
        // Retrieve the AudioSource component (if not already cached).
        // Note: It's more efficient to assign this in Awake or Start if used frequently.
        audiosource = GetComponent<AudioSource>();

        // Play the audio clip assigned to the AudioSource.
        audiosource.Play();
    }
}