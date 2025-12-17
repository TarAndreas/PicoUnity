using UnityEngine;
using Photon.Pun;

[ExecuteInEditMode]
public class Zoom : MonoBehaviour
{
    Camera camera;
    public float defaultFOV = 60;
    public float maxZoomFOV = 15;
    [Range(0, 1)]
    public float currentZoom;
    public float sensitivity = 1;
    PhotonView pv;


    void Awake()
    {
        // Get the camera on this gameObject and the defaultZoom.
        pv = GetComponent<PhotonView>();
        camera = GetComponent<Camera>();
        if (!pv.IsMine)
        {
            return;
        }   
        if (camera)
        {
            defaultFOV = camera.fieldOfView;
        }
    }

    void Update()
    {

        // Update the currentZoom and the camera's fieldOfView.
        currentZoom += Input.mouseScrollDelta.y * sensitivity * .05f;
        currentZoom = Mathf.Clamp01(currentZoom);
        camera.fieldOfView = Mathf.Lerp(defaultFOV, maxZoomFOV, currentZoom);
    }
}
