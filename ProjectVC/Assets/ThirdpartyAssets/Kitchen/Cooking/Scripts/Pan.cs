using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// This class represents a pan that can collect ingredients in a multiplayer (Photon) setting.
// It manages both the ingredient objects (GameObject) and their names (string), synchronizing state via RPC.
public class Pan : MonoBehaviour
{
    // List of ingredient names currently in the pan (shared over the network)
    public List<string> InhaltPanObjekt = new List<string>();

    // List of ingredient GameObjects currently in the pan (local only)
    public List<GameObject> InhaltePan = new List<GameObject>();

    // Reference to the attached PhotonView component for network communication
    PhotonView pv;

    // Called when the script is first enabled
    private void Start()
    {
        // Obtain the PhotonView component on this object (required for RPCs)
        pv = GetComponent<PhotonView>();
        // Clear the list of ingredient names at the start
        InhaltPanObjekt.Clear();
    }

    // Called by Unity when another collider/rigidbody enters a collision with this object
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object is tagged as an ingredient ("Zutat")
        if (collision.gameObject.tag == "Zutat")
        {
            // Add the ingredient GameObject to the local list
            InhaltePan.Add(collision.gameObject);

            // Tidy up the name: remove unwanted suffixes or clone indications
            var nameOfObject = collision.gameObject.name
                                              .Replace("(Clone)", "")
                                              .Replace("_positive", "")
                                              .Replace("_negative", "");

            // Synchronize addition of the ingredient name across all networked clients (buffered)
            pv.RPC("PanInhalt", RpcTarget.AllBuffered, nameOfObject);
        }
    }

    // This method is called over the network (via RPC) to add an ingredient name to the pan list
    [PunRPC]
    void PanInhalt(string InhaltZutat)
    {
        InhaltPanObjekt.Add(InhaltZutat);
    }
}