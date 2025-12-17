using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for cleaning up "ingredient" objects (tagged "Zutat") in the scene by marking them for removal,
/// and destroying them when the game ends. Designed for use in a cooking or slicing mini-game context.
/// </summary>
public class SliceCleaner : MonoBehaviour
{
    // Flag to indicate if the game has ended (helps prevent repeated calls to removal logic).
    private bool gameHasEnded = false;

    // Reference to the central Game state (must have a public bool endGame).
    public Game currentGame;

    void Update()
    {
        // Set gameHasEnded to match the currentGame's state.
        gameHasEnded = currentGame.endGame;
    }

    /// <summary>
    /// Called continuously while a Collider stays within this trigger.
    /// If the collider is tagged "Zutat", mark it for removal.
    /// If the game has ended, remove all marked objects.
    /// </summary>
    /// <param name="other">The collider currently within the trigger zone.</param>
    public void OnTriggerStay(Collider other)
    {
        // If the tagged object is an ingredient, mark it for removal.
        if (other.CompareTag("Zutat"))
        {
            other.gameObject.tag = "Remove";
        }

        // If the game has ended, remove all marked objects.
        // This is checked after any "Zutat" is marked, so cleanup is always possible.
        if (gameHasEnded)
        {
            removePieces();
            gameHasEnded = false; // Reset flag to avoid repeated removals.
        }
    }

    /// <summary>
    /// Destroys all GameObjects in the scene with the specified tag.
    /// </summary>
    /// <param name="tag">The tag to search for (e.g., "Remove").</param>
    public void removePieces()
    {

        //Destroy(GameObject.FindWithTag("Remove"));
        var piecesToRemove = GameObject.FindGameObjectsWithTag("Remove");

        foreach (var piece in piecesToRemove)
        {
            Destroy(piece);
        }
    }
}
