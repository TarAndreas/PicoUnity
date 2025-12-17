using System.Collections.Generic;
using UnityEngine;

// This script finds all GameObjects in the scene on a given layer (by name)
// and adds the "MoveWithLayer" component to them, if not already present.
public class FindGameObjectsByLayer : MonoBehaviour
{
    // The name of the layer to search for (can be set in the Inspector)
    public string layerName = "Interactable";

    // A list to store all found GameObjects on the specified layer
    public List<GameObject> gameObjectsall = new List<GameObject>();

    // Update is called once per frame
    private void Update()
    {
        // Find and update the list of all GameObjects on the given layer
        gameObjectsall = FindGameObjectsByLayerMethod();

        // For each GameObject found, ensure it has the MoveWithLayer component
        foreach (GameObject gameObject in gameObjectsall)
        {
            // If the GameObject does not already have the component, add it
            if (gameObject.GetComponent<MoveWithLayer>() == null)
            {
                gameObject.AddComponent<MoveWithLayer>();
            }
        }
    }

    // Finds all GameObjects (including children) in the scene which are on the target layer
    public List<GameObject> FindGameObjectsByLayerMethod()
    {
        List<GameObject> gameObjects = new List<GameObject>();

        // Find all GameObjects in the scene (including inactive objects)
        GameObject[] rootGameObjects = GameObject.FindObjectsOfType<GameObject>();

        // First, check each root GameObject for the target layer
        foreach (GameObject gameObject in rootGameObjects)
        {
            if (gameObject.layer == LayerMask.NameToLayer(layerName))
            {
                gameObjects.Add(gameObject);
            }
        }

        // Then, for each GameObject, check all its children recursively for the target layer
        foreach (GameObject rootGameObject in rootGameObjects)
        {
            // Get all child Transforms, including inactive children
            Transform[] childTransforms = rootGameObject.GetComponentsInChildren<Transform>(true);
            foreach (Transform childTransform in childTransforms)
            {
                if (childTransform.gameObject.layer == LayerMask.NameToLayer(layerName))
                {
                    gameObjects.Add(childTransform.gameObject);
                }
            }
        }

        return gameObjects;
    }
}