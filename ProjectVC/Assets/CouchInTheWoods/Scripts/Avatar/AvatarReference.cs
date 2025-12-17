using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Just holds some references needed for XRRig and Networking purpose
/// </summary>
public class AvatarReference : MonoBehaviour
{
    public GameObject avatarGameObject;
    public AvatarSeatController seatController;

    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor rayInteractor;
    public LineRenderer lineRenderer;
    public UnityEngine.XR.Interaction.Toolkit.Interactors.Visuals.XRInteractorLineVisual lineVisual;
    //public XRController controller;

    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor rayInteractor2;
    public LineRenderer lineRenderer2;
    public UnityEngine.XR.Interaction.Toolkit.Interactors.Visuals.XRInteractorLineVisual lineVisual2;
    //public XRController controller2;

    //public HandPresence handPresenceLeft;
    //public HandPresence handPresenceRight;

    /// <summary>
    /// Deprectaed because Toggle seat mode is not called via RPC anymore but within serializedNetworkSync
    /// </summary>
    [PunRPC]
    public void ToggleSeatRPC()
    {
        seatController.ToggleAvatars();
    }

    private void Update()
    {
        //Hands model and ray interactions need to be constantly forced to enabled = true because Android deactivates them somehow

        rayInteractor.enabled = true;
        lineRenderer.enabled = true;
        lineVisual.enabled = true;
        //controller.enabled = true;
        rayInteractor2.enabled = true;
        lineRenderer2.enabled = true;
        lineVisual2.enabled = true;
        //controller2.enabled = true;

        /*
        handPresenceLeft.gameObject.SetActive(true);
        handPresenceRight.gameObject.SetActive(true);
        handPresenceLeft.showController = false;
        handPresenceRight.showController = false;
        handPresenceLeft.enabled = true;
        handPresenceRight.enabled = true;
        */

        //Debug.Log(rayInteractor.gameObject.activeInHierarchy + " | " +
        //    rayInteractor.enabled + " | " +
        //        lineRenderer.enabled + " | " +
        //        lineVisual.enabled + " | " +
        //        //controller.enabled + " | " +
        //        rayInteractor2.enabled + " | " +
        //        lineRenderer2.enabled + " | " +
        //        lineVisual2.enabled + " | "
        //        /*controller2.enabled + " | "*/);
    }

}
