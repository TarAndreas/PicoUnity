using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;

// Enforces that this component can only be attached to objects with an Outline component attached.
[RequireComponent(typeof(Outline))]
public class SimpleOutliner : MonoBehaviour
{
    // Reference to the Outline component, which provides a visual outline effect.
    private Outline outline;

    // Color for the outline when the seat is hovered over.
    public Color seatHoveredColor = new Color(0, 255, 0, 1);
    // Color for the outline when the seat is not hovered (default: transparent).
    public Color seatNotHovered = new Color(0, 0, 0, 0);

    // Reference to the XRGrabInteractable component for interaction events in XR.
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabinter;

    /// <summary>
    /// Called when the script instance is being loaded.
    /// Initializes references to required components.
    /// </summary>
    private void Awake()
    {
        grabinter = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        outline = GetComponent<Outline>();
    }

    /// <summary>
    /// Unity's Start method�sets up event listeners and initial outline state.
    /// </summary>
    void Start()
    {
        // Subscribe to interaction events from XRGrabInteractable.
        grabinter.hoverEntered.AddListener(OnSeatHovered);
        grabinter.hoverExited.AddListener(OnSeatHoverExit);

        // Set initial outline appearance: invisible but a defined width (for consistency).
        outline.OutlineColor = seatNotHovered;
        outline.OutlineWidth = 10f;
    }

    /// <summary>
    /// Event handler for when another XR interactor hovers over this object.
    /// Makes the outline visible and changes its color to indicate it's being hovered.
    /// </summary>
    public void OnSeatHovered(HoverEnterEventArgs args)
    {
        outline.enabled = true;
        outline.OutlineColor = seatHoveredColor;
    }

    /// <summary>
    /// Event handler for when the hover interaction ends.
    /// Disables the outline and restores its color to 'not hovered' (transparent).
    /// </summary>
    public void OnSeatHoverExit(HoverExitEventArgs args)
    {
        outline.enabled = false;
        outline.OutlineColor = seatNotHovered;
    }
}