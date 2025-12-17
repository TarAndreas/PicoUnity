using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Voice.Unity;
using Photon.Realtime;

/// <summary>
/// This script draws a synchronized line in 3D space using Unity's LineRenderer to visualize the character's path.
/// Line drawing is networked using Photon PUN to ensure all players see the same trajectory.
/// </summary>
public class PathLineRenderer : MonoBehaviour
{
    // Reference to the LineRenderer component in the scene (assign in Inspector)
    [SerializeField] private LineRenderer lineRenderer;

    // Reference to the GameObject representing the character whose path will be drawn (assign in Inspector)
    [SerializeField] private GameObject character;

    // Stores the last known position of the character to detect movement
    private Vector3 currentposition;

    // Tracks the number of points in the line (used to manage the LineRenderer positions)
    [SerializeField] int linrendercount;

    // Used to control update frequency for path drawing (seconds between samples)
    float wait = 2.0f;

    /// <summary>
    /// Initialize the line position count at start (optionally, you may set the initial position here).
    /// </summary>
    void Start()
    {
        linrendercount = 0;
    }

    /// <summary>
    /// Called every frame. Checks if the character has moved; if so and the wait timer allows,
    /// triggers a networked update to add a new point to the path.
    /// </summary>
    void Update()
    {
        // Only proceed if the position of the character has changed since the last check
        if (currentposition != character.transform.position)
        {
            // Countdown the wait timer
            if (wait > 0)
            {
                wait -= Time.deltaTime;
            }
            // Once the timer has elapsed, draw the next segment on all clients
            if (wait < 0)
            {
                PhotonView pv = PhotonView.Get(this);
                // Call the line drawing routine across all clients for synchronization
                pv.RPC("RpcLineDraw", RpcTarget.All);
                // Reset the timer, so the path is drawn at a manageable interval
                wait = 1.0f;
            }
        }
    }

    /// <summary>
    /// Photon RPC to add a new point to the LineRenderer for path visualization.
    /// This method is called on all clients to preserve visual consistency.
    /// </summary>
    [PunRPC]
    public void RpcLineDraw()
    {
        lineRenderer.positionCount = linrendercount + 1; // Add another point slot
        linrendercount++;
        currentposition = character.transform.position; // Update last known position
        // Place the new point at the character's current position
        lineRenderer.SetPosition(linrendercount - 1, character.transform.position);
    }

    /// <summary>
    /// Public method to reset the line on all clients (can be called from UI or other scripts).
    /// </summary>
    public void emptyLineRenderer()
    {
        PhotonView currentPV = PhotonView.Get(this);
        currentPV.RPC("RpcEmptyLR", RpcTarget.All);
    }

    /// <summary>
    /// Photon RPC to clear the line by setting all points to zero.
    /// </summary>
    [PunRPC]
    public void RpcEmptyLR()
    {
        Vector3 vectorpos = new Vector3(0.0f, 0.0f, 0.0f);

        while (linrendercount != 0)
        {
            linrendercount--;
            // Move each point back to the origin (could also clear positionCount, depending on design)
            lineRenderer.SetPosition(linrendercount - 1, vectorpos);
        }
    }
}