using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable))]
[RequireComponent(typeof(Outline))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class SeatProvider : MonoBehaviour
{

    [SerializeField] private bool isSeatOccupied;
    [SerializeField] public bool IsSeatOccupied { get => isSeatOccupied; set => isSeatOccupied = value; }


    //[SerializeField] private bool isSeatTriggerEnterdbyPlayer;

    public Transform seat_Target;
    private Collider seat_Trigger;

    private float triggerDistance = 5;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable interactable;
    private Rigidbody body;

    private Outline outline;
    private Color seatHoveredColor = new Color(0, 255, 0, 1);
    private Color seatNotHovered = new Color(0, 0, 0, 0);


    private void Awake()
    {
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
        outline = GetComponent<Outline>();
        body = GetComponent<Rigidbody>();
        seat_Trigger = GetComponent<Collider>();

    }


    // Start is called before the first frame update
    void Start()
    {
        SetupSeat();
    }


    private void SetupSeat()
    {
        // COLLIDER
        seat_Trigger.isTrigger = true;

        // RIGIDBODY
        body.isKinematic = true;
        body.useGravity = false;

        // INTERACTABLE
        
        interactable.hoverEntered.AddListener(OnSeatHovered);
        interactable.hoverExited.AddListener(OnSeatHoverExit);
        interactable.activated.AddListener(OnSeatClicked);

        // OUTLINE
        outline.OutlineColor = seatNotHovered;            
        outline.OutlineWidth = 10f;


        // SEAT TARGET if lost
        if (transform.GetComponent<Transform>().Find("SeatTarget") == null)
        {
            GameObject seatTarget = new GameObject();
            seatTarget.transform.position = new Vector3(0, 0.5f, 0);
            seatTarget.transform.rotation = Quaternion.identity;

        }


    }


    public void OnSeatHovered(HoverEnterEventArgs args)
    {
        if (triggerDistance < Vector3.Distance(seat_Target.transform.position, args.interactorObject.transform.position))
            return;

        outline.enabled = true;
        outline.OutlineColor = seatHoveredColor;

    }

    public void OnSeatHoverExit(HoverExitEventArgs args)
    {

        outline.enabled = false;
        outline.OutlineColor = seatNotHovered;
    }

    public void OnSeatClicked(ActivateEventArgs args)
    {
        if (IsSeatOccupied)
            return;

        IsSeatOccupied = true;

        args.interactorObject.transform.GetComponentInParent<AvatarReference>().avatarGameObject.GetComponent<AvatarSeatController>().SetNewSeatTarget(transform.gameObject);

        outline.enabled = false;
        outline.OutlineColor = seatNotHovered;

    }

}
