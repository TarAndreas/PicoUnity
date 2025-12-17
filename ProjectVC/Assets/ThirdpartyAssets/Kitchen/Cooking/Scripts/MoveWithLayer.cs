using UnityEngine;

// This script allows the attached GameObject to be "grabbed" with the mouse and moved in 3D space along the ray from the camera.
// It disables gravity while the object is grabbed and restores it on release.
public class MoveWithLayer : MonoBehaviour
{
    private Rigidbody _grabbedObject;

    void Update()
    {
        // Detect mouse press (left button down)
        if (Input.GetMouseButtonDown(0))
        {
            // Create a ray from the camera to the point where the mouse cursor is
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Perform the raycast to check if an object is hit
            if (Physics.Raycast(ray, out hit))
            {
                // Only proceed if this script's GameObject was hit
                if (hit.collider.gameObject == this.gameObject)
                {
                    // Get the Rigidbody component (required for movement)
                    _grabbedObject = GetComponent<Rigidbody>();
                    if (_grabbedObject != null)
                    {
                        // Disable gravity while the object is held
                        _grabbedObject.useGravity = false;
                    }
                }
            }
        }

        // Detect mouse release (left button up)
        if (Input.GetMouseButtonUp(0))
        {
            // If we had grabbed an object, re-enable gravity and clear reference
            if (_grabbedObject != null)
            {
                _grabbedObject.useGravity = true;
                _grabbedObject = null;
            }
        }

        // If currently grabbing an object, move it to follow the mouse/cursor in 3D space
        if (_grabbedObject != null)
        {
            // Cast a ray from the camera through the current mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Find a point along the ray to position the object (1 unit from the camera)
            Vector3 newPosition = ray.GetPoint(1);

            // Move the object smoothly to the new position
            _grabbedObject.MovePosition(newPosition);
        }
    }
}