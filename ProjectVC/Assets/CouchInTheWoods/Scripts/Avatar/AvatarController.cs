using UnityEngine;
using UnityEngine.Animations.Rigging;

[System.Serializable]
public class MapTransform
{
    public Transform vrTarget;
    public Transform IKTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;

    public void MapVRAvatar()
    {
        IKTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
        IKTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    }
}

[RequireComponent(typeof(BoneRenderer))]
[RequireComponent(typeof(RigBuilder))]
public class AvatarController : MonoBehaviour
{
    [SerializeField] private MapTransform head;
    [SerializeField] private MapTransform leftHand;
    [SerializeField] private MapTransform rightHand;

    [SerializeField] public float turnSmoothness;

    [SerializeField] private Transform IKHead;

    [SerializeField] private Vector3 headBodyOffset;

    public Vector3 head_positionMin;
    public Vector3 head_positionMax;

    public bool clampPositionHeight;

    private AvatarSeatController avatarSeatController;

    private void Start()
    {
        avatarSeatController = GetComponentInParent<AvatarSeatController>();
    }

    /// <summary>
    /// Maps the IK targets to the correction positions
    /// </summary>
    void LateUpdate()
    {
        if(IKHead != null && !avatarSeatController.isAvatarSeat)
        {
            transform.position = IKHead.position;
            transform.forward = Vector3.Lerp(transform.forward, Vector3.ProjectOnPlane(IKHead.forward, Vector3.up).normalized, Time.deltaTime * turnSmoothness);

            transform.position += (transform.rotation * headBodyOffset);
        }

        head.MapVRAvatar();
        leftHand.MapVRAvatar();
        rightHand.MapVRAvatar();
    }
}