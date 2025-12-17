using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindSceneCamera : MonoBehaviour
{
    Camera sceneCamera;
    public Canvas canvas;


    // Start is called before the first frame update
    void Start()
    {
        AssignSceneCamera();
    }

    /// <summary>
    /// Find camera in scene and assign it to the cavas as global camera
    /// </summary>
    public void AssignSceneCamera()
    {
        GameObject cameraGO = GameObject.FindWithTag("MainCamera");
        sceneCamera = cameraGO.GetComponent<Camera>();
        canvas.worldCamera = sceneCamera;
    }
}
