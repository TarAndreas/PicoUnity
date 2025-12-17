using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This script triggers particle and audio effects (and can be extended for other effects)
/// when an object enters or exits a trigger collider.
/// It also includes a UnityEvent for custom events, although this is not used in the current implementation.
/// </summary>
public class TriggerEventHandler : MonoBehaviour
{
    // Defines a UnityEvent that can pass an integer parameter, allowing for flexible event calls in future use.
    [System.Serializable]
    public class EventList : UnityEvent<int> { }
    public EventList StartEvents;

    // Reference to the GameObject whose child components (like ParticleSystem, AudioSource) will be activated.
    [SerializeField]
    public new GameObject eventObject;

    /// <summary>
    /// Unity callback: triggered when another collider enters this GameObject's trigger.
    /// Starts effects.
    /// </summary>
    public void OnTriggerEnter(Collider col)
    {
        PlayCustomEvents();
        // Optionally, you could invoke custom events: StartEvents.Invoke(0);
    }

    /// <summary>
    /// Unity callback: triggered when another collider exits this GameObject's trigger.
    /// Stops effects.
    /// </summary>
    public void OnTriggerExit(Collider col)
    {
        StopCustomEvents();
    }

    /// <summary>
    /// Plays the first found ParticleSystem and AudioSource among target GameObject and its children.
    /// </summary>
    public void PlayCustomEvents()
    {
        var particle = eventObject.GetComponentInChildren<ParticleSystem>();
        if (particle != null)
            particle.Play();

        var audio = eventObject.GetComponentInChildren<AudioSource>();
        if (audio != null)
            audio.Play();
    }

    /// <summary>
    /// Stops the first found ParticleSystem and AudioSource among target GameObject and its children.
    /// </summary>
    public void StopCustomEvents()
    {
        var particle = eventObject.GetComponentInChildren<ParticleSystem>();
        if (particle != null)
            particle.Stop();

        var audio = eventObject.GetComponentInChildren<AudioSource>();
        if (audio != null)
            audio.Stop();
    }
}