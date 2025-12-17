using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationContoller : MonoBehaviour
{
    Animator animator;

    public InputActionReference leftHand;

    public Transform mainCamera;
    public Vector3 previousPosition;
    public Vector3 previousRotation;

    public float deltaMultipler;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Update animation values depending on player movement to play walking animations
    /// </summary>
    private void Update()
    {
        // Set Position
        var currentFramePosition = mainCamera.position;
        Vector3 deltaPosition = currentFramePosition - previousPosition;
        deltaPosition = Quaternion.Inverse(mainCamera.rotation) * deltaPosition;       
        SetAnimationVector(deltaPosition * deltaMultipler);
        previousPosition = mainCamera.position;
    }


    public void SetAnimationVector(Vector3 vector3)
    {
        animator.SetFloat("sideways", vector3.x);
        animator.SetFloat("forward", vector3.z);
    }
}
