/*
 * Add Listener to a variable and call method once invoked.
 * Change the owner for an interactive XR object
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AddScriptToGameObject : MonoBehaviour
{
    private OwnerTakeOver newOwner;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable selecter;

    //Assign the GameObject with the XRGrabInteractable
    private void Awake()
    {
        selecter = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
    }

    //Add Listener to the component and call method once invoked
    void Start()
    {
        selecter.selectEntered.AddListener(AddArgument);
    }

    //The method, the listener is going to call
    //Change the status for a grabbed object as selected
    public void AddArgument(SelectEnterEventArgs args)
    {
        newOwner.OnSelectEnter();
    }
}
