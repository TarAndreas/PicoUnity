/*
 * Place a texture infront of an assigned camera object
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignCameraTexture : MonoBehaviour
{
    //Assign a camera object the texture is added to
    public GameObject playerCam;
    //Texture that is used
    public Vector3 TextureOffset;
    public Transform CameraToTexture;
    private bool activeTexture = false;
    public GameObject CanvasPrefab;

    //Get the camera, its transform values and rotation. Adjust the variables for the texture
    //Instatiate the canvas with texture on the position
    public void PlaceTextureOnCamera()
    {
        playerCam = GameObject.FindWithTag("MainCamera");
        CameraToTexture = playerCam.transform;


        Debug.Log("Camera X value: " + CameraToTexture.position.x);
        Debug.Log("Camera Y value: " + CameraToTexture.position.y);
        Debug.Log("Camera Z value: " + CameraToTexture.position.z);

        //Set adjusted position for the texture, so its not inside the camera
        Vector3 changedPos = CameraToTexture.position + TextureOffset;
        //Set rotation to zero
        Quaternion rotations = new Quaternion();
        rotations.x = 0.0f;
        rotations.y = 0.0f;
        rotations.z = 0.0f;
        //Create canvas with adjusted position and rotation
        Instantiate(CanvasPrefab, changedPos, rotations, playerCam.transform);
        activeTexture = true;
        

    }
    //Check if texture is active, if not deactivate canvas on camera
    public void StateOfTexture()
    {
        if(!activeTexture)
        {
            Debug.Log("Texture is not active");
        } else
        {
            GameObject instCanvas = GameObject.Find("CanvasCameraMask(Clone)");
            instCanvas.SetActive(false);
            activeTexture = false;
        }
    }
    //Place texture infrot of the camera object, if there is no texture 
    public void StateOfCanvas()
    {
 
        if(!activeTexture)
        {
            PlaceTextureOnCamera();
            
        } else
        {
            StateOfTexture();
        }
    }
}
