using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script checks at every frame if VR mode is enabled.
// It activates/deactivates GameObjects accordingly and updates positions based on the VR status.
public class HeadsetCheck : MonoBehaviour
{

    public GameObject VRHeadset;
    public GameObject FristPerson;
    public GameObject Capsol;
    public GameObject Avatar;
    public GameObject PlayerBody;

    void Update()
    {
        // If VR is active in the project settings
        if (UnityEngine.XR.XRSettings.enabled)
        {
            // Deactivate the non-VR first person controller
            FristPerson.SetActive(false);
            
            // Activate the VR headset representation in the scene
            VRHeadset.SetActive(true);
        }
        else
        {
            // If VR is not enabled, activate the first person controller
            VRHeadset.SetActive(false);
            FristPerson.SetActive(true);

            // Move the Avatar and PlayerBody to the position of the Capsule
            Avatar.transform.position = Capsol.transform.position;
            PlayerBody.transform.position = Capsol.transform.position;
        }
        
    }
}