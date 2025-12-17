using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; // For multiplayer RPC functionality

// This component handles "cutting" mechanics for objects marked as "destroyable" in a Photon/VR environment.
public class Cut : MonoBehaviour
{

    private Transform DestroyGameObject;
    private PhotonView pv;
    [SerializeField] public GameObject kneif;
    [SerializeField] public GameObject Sliceable;

    // Prefabs or game objects representing the "sliced" pieces to instantiate upon cutting
    [SerializeField] public List<GameObject> sclides = new List<GameObject>();

    // Awake() or Start() is called when the script instance is being loaded
    private void Start()
    {
        // Find all objects in the scene tagged as "Destroyable"
        var alleGameobjects = GameObject.FindGameObjectsWithTag("Destroyable");

        // Iterate through them and find the one called "DestroyGameObject" or its "(Clone)"
        foreach (GameObject gameObject in alleGameobjects)
        {
            if (gameObject.name == "DestroyGameObject" || gameObject.name == "DestroyGameObject(Clone)")
            {
                // Cache its transform for parenting future slices
                DestroyGameObject = gameObject.transform;
            }
        }
    }

    // Called by Unity when this collider/rigidbody starts colliding with another collider
    public void OnCollisionEnter(Collision collision)
    {

        Debug.Log("on Collation"); // Logs that collision was detected
        Debug.Log(collision.gameObject.name); // Logs the name of the colliding object

        // If the colliding object is tagged as "Knife"
        if (collision.gameObject.tag == "Knife")
        {
            // Deactivate the sclicble (the object to cut)
            Sliceable.SetActive(false);

            // For each "sliced" piece, spawn it at the position of the sclicble
            foreach (GameObject a in sclides)
            {
                var slice = Instantiate(a, Sliceable.transform.position, Quaternion.identity);
                // Parent the new slice under the destroy object
                slice.transform.parent = DestroyGameObject;
                // Activate the (Prefab? might be unnecessary if newly instantiated)
                a.SetActive(true);
            }
        }
    }

    // RPC function for cut action across the network
    [PunRPC]
    void RPC_Instantiate(Collision collision)
    {
        if (collision.gameObject.tag == "Knife")
        {
            Sliceable.SetActive(false);
            foreach (GameObject a in sclides)
            {
                var slice = Instantiate(a, Sliceable.transform.position, Quaternion.identity);
                slice.transform.parent = DestroyGameObject;
                a.SetActive(true);
            }
        }
    }
}