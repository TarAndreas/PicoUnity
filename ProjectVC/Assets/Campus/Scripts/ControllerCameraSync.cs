/*
 * Change position of the camera inside of the XRRig, so its actually at head level
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerCameraSync : MonoBehaviour
{
    
    public GameObject cameraPosition;
    public CharacterController xrRigController;
    private float xValue;
    private float zValue;
    private float yValue;
    public float adjustXValue = 0.0f;
    public float adjustYValue = 0.5f;
    public float adjustZValue = 0.5f;
    private Vector3 wantedCenterPosition;

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        //xrRigController.center = cameraPosition.transform.localPosition;


        
        xValue = cameraPosition.transform.localPosition.x - adjustXValue;
        zValue = cameraPosition.transform.localPosition.z - adjustZValue;
        yValue = cameraPosition.transform.localPosition.y - adjustYValue;
        wantedCenterPosition = new Vector3(xValue, yValue, zValue);
        xrRigController.center = wantedCenterPosition;
        

    }
}
