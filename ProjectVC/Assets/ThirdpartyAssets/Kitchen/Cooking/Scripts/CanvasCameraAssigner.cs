using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasCameraAssigner : MonoBehaviour
{
    // Reference to the parent GameObject of the player
    private GameObject parentTransform;

    // Reference to the player's "First Person Controller" transform
    private Transform firstPerson;

    // Public transforms to which the player can be teleported (assigned in the Inspector)
    public Transform kitchen;
    public Transform Team1;
    public Transform Team2;
    public Transform way;
    public Transform Ballraum;

    // Unity's Awake method is called when the script instance is loaded
    void Awake()
    {
        // Find the first Camera in the scene
        Camera mainCamera = FindObjectOfType<Camera>();

        // If a camera is found, assign it to the Canvas component for rendering
        if (mainCamera != null)
        {
            // Get the Canvas component on the same GameObject as this script
            Canvas canvas = GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera; // Set canvas to render in camera space
            canvas.worldCamera = mainCamera;                  // Assign the main camera to the canvas
        }
    }

    // Moves the player to the position of the "kitchen" transform
    public void TeleportKitchen()
    {
        // Find the VR player GameObject by its specific name
        parentTransform = GameObject.Find("VRPlayerPrefab_VAR7_ActionBased(Clone)");

        // Find the child "First Person Controller" transform
        firstPerson = parentTransform.transform.Find("First Person Controller");

        if (firstPerson != null)
        {
            // Set position to kitchen's position
            firstPerson.position = kitchen.position;
        }
        else
        {
            Debug.Log("dont find the firstperson"); // Log if not found
        }
    }

    // Moves the player to the position of the "Team1" transform
    public void TeleportTeam1()
    {
        parentTransform = GameObject.Find("VRPlayerPrefab_VAR7_ActionBased(Clone)");
        firstPerson = parentTransform.transform.Find("First Person Controller");
        if (firstPerson != null)
        {
            firstPerson.position = Team1.position;
        }
        else
        {
            Debug.Log("dont find the firstperson");
        }

    }

    // Moves the player to the position of the "Team2" transform
    public void TeleportTeam2()
    {
        parentTransform = GameObject.Find("VRPlayerPrefab_VAR7_ActionBased(Clone)");
        firstPerson = parentTransform.transform.Find("First Person Controller");
        if (firstPerson != null)
        {
            firstPerson.position = Team2.position;
        }
        else
        {
            Debug.Log("dont find the firstperson");
        }

    }

    // Moves the player to the position of the "way" transform
    public void Teleportway()
    {
        parentTransform = GameObject.Find("VRPlayerPrefab_VAR7_ActionBased(Clone)");
        firstPerson = parentTransform.transform.Find("First Person Controller");
        if (firstPerson != null)
        {
            firstPerson.position = way.position;
        }
        else
        {
            Debug.Log("dont find the firstperson");
        }

    }

    // Moves the player to the position of the "Ballraum" transform
    public void TeleportBallraum()
    {
        parentTransform = GameObject.Find("VRPlayerPrefab_VAR7_ActionBased(Clone)");
        firstPerson = parentTransform.transform.Find("First Person Controller");
        if (firstPerson != null)
        {
            firstPerson.position = Ballraum.position;
        }
        else
        {
            Debug.Log("dont find the firstperson");
        }

    }
}