using Photon.Pun;                  // Used for Photon Networking (Remote Procedure Calls etc.)
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;        // Not directly used in this script, can be removed if unnecessary
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit; // Not directly used in this script, can be removed if unnecessary

public class Pizza : MonoBehaviour
{
    // List of ingredient names currently on the pizza (will be kept in sync across all clients)
    public List<string> InhaltDerPizza = new List<string>();

    // Local list of ingredient GameObjects on this pizza instance (not synchronized over network)
    public List<GameObject> PizzenInhalte = new List<GameObject>();

    // Reference to PhotonView for network operations on this GameObject
    PhotonView pv;

    // Called when the script instance is being loaded
    private void Start()
    {
        // Get PhotonView component attached to this GameObject for networking/RPC functionality
        pv = GetComponent<PhotonView>();

        // Ensure the ingredient name list is empty at the start
        InhaltDerPizza.Clear();
    }

    // Called automatically by Unity when another collider makes contact with this collider (if one has Rigidbody)
    private void OnCollisionEnter(Collision collision)
    {
        // Only do something if the other object represents an ingredient ("Zutat")
        if (collision.gameObject.tag == "Zutat")
        {
            // Add the GameObject itself to the local object list (local, not networked)
            PizzenInhalte.Add(collision.gameObject);

            // Clean up the object name by removing "(Clone)", "_positive" and "_negative" suffixes
            var nameOfObject = collision.gameObject.name
                                           .Replace("(Clone)", "")
                                           .Replace("_positive", "")
                                           .Replace("_negative", "");

            // Send an RPC to all clients to add this ingredient name to the shared ingredient list
            pv.RPC("PizzaInhalt", RpcTarget.AllBuffered, nameOfObject);
        }
    }

    // Makes sure all clients add the ingredient name string to their individual lists
    [PunRPC]
    void PizzaInhalt(string InhaltZutat)
    {
        InhaltDerPizza.Add(InhaltZutat);
    }
}