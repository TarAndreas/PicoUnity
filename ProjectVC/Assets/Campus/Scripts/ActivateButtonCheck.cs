/*
 * Check if the correct button is shown inside the laboratory task
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateButtonCheck : MonoBehaviour
{

    [SerializeField]
    public GameObject ObservedObject;

    [SerializeField]
    public GameObject YellowButton;

    [SerializeField]
    public GameObject RedButton;

    [SerializeField]
    public GameObject GreenButton;

    //Check if Object is active else activate other buttons
    void Update()
    {
        if(ObservedObject.activeSelf)
        {
            ChangeButtonStatus();
        } else
        {
            RedButton.SetActive(false);
            GreenButton.SetActive(true);
        }
    }
    //Set boolean variable for the buttons
    public void ChangeButtonStatus()
    {
        RedButton.SetActive(true);
        YellowButton.SetActive(false);
        GreenButton.SetActive(false);
    }

}
