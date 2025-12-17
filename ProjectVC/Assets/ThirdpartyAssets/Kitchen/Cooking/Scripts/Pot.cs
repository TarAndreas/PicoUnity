using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting; // (Unused in this script - can be removed if not needed elsewhere)
using UnityEngine;
using Photon.Pun; // Photon Networking for synchronizing content

// This class represents a pot (cooking vessel) in a networked scene,
// allowing ingredients ("Zutat") to be added to or removed from it, syncing their names over the network.
public class Pot : MonoBehaviour
{
    // List of ingredient names currently inside the pot, synchronized across all clients
    public List<string> InhaltDesTopfes = new List<string>();

    // Local list of ingredient GameObjects in the pot (not synchronized)
    public List<GameObject> ToepfeInhalte = new List<GameObject>();

    // Reference to the PhotonView (for RPC calls)
    PhotonView pv;

    // Called when the script instance is initialized
    private void Start()
    {
        pv = GetComponent<PhotonView>(); // Get the PhotonView component attached to this object
        InhaltDesTopfes.Clear();         // Ensure the string list starts empty
    }

    // (Optional) Called every frame for colliders in contact; currently unused, can be removed if not used later
    private void OnCollisionStay(Collision collision)
    {

    }

    // Called by Unity when another collider/rigidbody enters contact with this object
    private void OnCollisionEnter(Collision collision)
    {
        // If the contacting object is tagged as "Zutat" (ingredient)...
        if (collision.gameObject.tag == "Zutat")
        {
            // Add the GameObject to the local list (for local logic, not synced)
            ToepfeInhalte.Add(collision.gameObject);

            // Clean up the object's name ('(Clone)', '_positive', '_negative' substrings)
            var nameOfObject = collision.gameObject.name
                .Replace("(Clone)", "")
                .Replace("_positive", "")
                .Replace("_negative", "");

            // Send an RPC so all clients add this ingredient name to the shared list
            pv.RPC("PotInhalt", RpcTarget.AllBuffered, nameOfObject);
        }
    }

    // Called when another collider stops touching this object's collider
    private void OnCollisionExit(Collision collision)
    {
        // Always try to remove the GameObject from the local object list
        ToepfeInhalte.Remove(collision.gameObject);

        // Attempt to remove the ingredient name from the synced list using its current name
        InhaltDesTopfes.Remove(collision.gameObject.name); // (Note: this may NOT match the normalized name stored earlier)
    }

    // RPC method to be called on all clients,
    // ensuring that the name of the ingredient is added to the pot contents everywhere
    [PunRPC]
    void PotInhalt(string InhaltZutat)
    {
        InhaltDesTopfes.Add(InhaltZutat);
    }
}
