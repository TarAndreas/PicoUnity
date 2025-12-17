using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class AvatarSeatController : MonoBehaviour
{
    public float offsetScaling;

    [Header("Avatars")]
    public GameObject avatarGameObject;
    private Vector3 avatarWalkPosition;
    /// <summary>
    /// Avatar offset when sitting
    /// </summary>
    public Vector3 avatarSeatPositionOffset;

    [Header("Rig")]
    public GameObject vrOriginGameObject;
    private Vector3 vrOriginWalkPosition;
    /// <summary>
    /// Offset for the XR Rig when sitting
    /// </summary>
    public Vector3 vrOriginPositionOffset;

    [Header("Rig Controller")]
    public GameObject mainCamera;
    private Vector3 mainCameraPosition;
    /// <summary>
    /// Offset for the main cam when sitting
    /// </summary>
    public Vector3 mainCameraPositionOffset;
    public GameObject mainHead;

    [Header("Constraint")]
    public TwoBoneIKConstraint leftArm;
    public TwoBoneIKConstraint rightArm;

    [Header("Animator")]
    //Bool required for switching the teleportation provider on/off when sitting /standing
    private bool isTeleportationProviderOn = true;
    public Animator animator;
    public RuntimeAnimatorController seatAnimation;
    public RuntimeAnimatorController walkAnimation;
    public Avatar walkAvatar;
    public Avatar seatAvatar;

    [Header("Seat Stuff")]
    public bool isAvatarSeat;
    private GameObject currentSeat;
    public Transform seatTarget_transform;
    public Vector3 seatTargetOffset;

    [Header("Scripts to take off and change")]

    public TrackedPoseDriver trackedPoseDriver_mainCamera;
    public LowerBodyAnimation lowerBodyAnimation;
    public AvatarController avatarController;
    public ContinuousMoveProviderBase continuousMoveProviderBase;
    public UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationProvider teleportationProvider;


    [Header("Player Input")]
    public InputActionReference leftHand;
    public InputActionReference rightHand;

    private AvatarNetworkSync networkSync;

    private Vector3 originalvrOriginPositionOffset;

    private void Awake()
    {
        networkSync = GetComponent<AvatarNetworkSync>();
    }

    private void Start()
    {
        originalvrOriginPositionOffset = vrOriginPositionOffset;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAvatarSeat == true)
        {
            //Setting position to seat when in sitting mode
            SetPositions();

            var leftHandValue = leftHand.action?.ReadValue<Vector2>() ?? Vector2.zero;
            var rightHandValue = rightHand.action?.ReadValue<Vector2>() ?? Vector2.zero;

            //Automatically stand up when moving away from seat
            if (leftHandValue.magnitude > 0.5 || rightHandValue.magnitude > 0.5)
                ToggleAvatars();
        }
    }

    /// <summary>
    /// Called From Seat Provider when clicked
    /// </summary>
    /// <param name="seat"></param>
    public void SetNewSeatTarget(GameObject seat)
    {
        currentSeat = seat;
        seatTarget_transform = seat.GetComponent<SeatProvider>().seat_Target.transform;

        ToggleAvatars();
    }

    /// <summary>
    /// Called From Seat and From Update
    /// </summary>
    [ContextMenu("Reset Position")]
    public void ToggleAvatars()
    {
        if (avatarGameObject == null)
            return;

        teleportationProvider.enabled = isTeleportationProviderOn;

        /// WALK TO SEAT
        if (!isAvatarSeat)
        {
            networkSync.SetSittingState(true);

            // Set Seat
            if (currentSeat != null)
            {
                currentSeat.GetComponent<SeatProvider>().IsSeatOccupied = true;
                isAvatarSeat = currentSeat.GetComponent<SeatProvider>().IsSeatOccupied;
            }

            // Stop Body Movement & Animation
            lowerBodyAnimation.isLowerBodyAnimationActive = false;
            continuousMoveProviderBase.enabled = false;

            // Set Animator to Seat
            animator.avatar = seatAvatar;
            animator.runtimeAnimatorController = seatAnimation;
            animator.Rebind();

            // Reset Teleport
            if (isTeleportationProviderOn == true || isTeleportationProviderOn == false)
                teleportationProvider.enabled = false;


            // SAVE AVATAR DATA
            avatarWalkPosition = avatarGameObject.transform.position;

            // SAVE RIG DATA
            vrOriginWalkPosition = vrOriginGameObject.transform.position;

            //SAVE CONTROLLERS
            mainCameraPosition = mainCamera.transform.position;

            SetPositions();
        }
        /// BACK TO WALK
        else
        {
            networkSync.SetSittingState(false);

            // Set Seat
            if (currentSeat != null)
            {
                currentSeat.GetComponent<SeatProvider>().IsSeatOccupied = false;
                isAvatarSeat = currentSeat.GetComponent<SeatProvider>().IsSeatOccupied;
            }

            // Stop Body Movement & Animation
            lowerBodyAnimation.isLowerBodyAnimationActive = true;
            continuousMoveProviderBase.enabled = true;

            // Set Animator to Seat
            animator.avatar = walkAvatar;
            animator.runtimeAnimatorController = walkAnimation;
            animator.Rebind();

            vrOriginPositionOffset = originalvrOriginPositionOffset;

            // Reset Teleport
            if (isTeleportationProviderOn == true || isTeleportationProviderOn == false)
                teleportationProvider.enabled = true;

            //RESET AVATAR POSITION
            avatarGameObject.transform.position = avatarWalkPosition;


            //RESET RIG POSITION
            vrOriginGameObject.transform.position = vrOriginWalkPosition;

            //// RESET CAMERA
            mainCamera.transform.position = mainCameraPosition;

            // RESET CONSTRAINT
            leftArm.data.hintWeight = 1;
            rightArm.data.hintWeight = 1;
        }
    }

    /// <summary>
    /// Set avatar position to seat
    /// </summary>
    private void SetPositions()
    {
        if (seatTarget_transform == null)
            return;

        SetPositions(seatTarget_transform.position, seatTarget_transform.rotation);
    }

    /// <summary>
    /// Set avatar position to seat
    /// </summary>
    public void SetPositions(Vector3 position, Quaternion rotation)
    {
        // SET AVATAR POSITION + OFFSET
        avatarGameObject.transform.position = position + avatarSeatPositionOffset;
        avatarGameObject.transform.rotation = rotation;

        //// SET RIG POSITION + OFFSET
        vrOriginGameObject.transform.position = position + vrOriginPositionOffset;
        vrOriginGameObject.transform.rotation = rotation;


        // SET CONSTRAINT
        leftArm.data.hintWeight = 0;
        rightArm.data.hintWeight = 0;

        // SET ORIGIN
        mainCamera.transform.position = position + mainCameraPositionOffset;
        mainCamera.transform.rotation = rotation;
    }
}
