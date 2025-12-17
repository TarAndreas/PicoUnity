using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LowerBodyAnimation : MonoBehaviour
{
    public bool isLowerBodyAnimationActive = true;

    [SerializeField] private Animator animator;

    [SerializeField][Range(0, 1)] private float leftFootPositionWeight;
    [SerializeField][Range(0, 1)] private float rightFootPositionWeight;

    [SerializeField][Range(0, 1)] private float leftFootRotationWeight;
    [SerializeField][Range(0, 1)] private float rightFootRotationWeight;

    public Vector3 footOffset;
    public Vector3 raycastLeftOffset;
    public Vector3 raycastRightOffset;


    private void OnAnimatorIK(int layerIndex)
    {
        if (!isLowerBodyAnimationActive)
            return;

        RaycastHit hitLeftFoot;
        RaycastHit hitRightFoot;

        Vector3 leftFootPosition = animator.GetIKPosition(AvatarIKGoal.LeftFoot);
        Vector3 rightFootPosition = animator.GetIKPosition(AvatarIKGoal.RightFoot);

        bool isLeftFootDown = Physics.Raycast(leftFootPosition + raycastLeftOffset, Vector3.down, out hitLeftFoot);
        bool isRightFootDown = Physics.Raycast(rightFootPosition + raycastRightOffset, Vector3.down, out hitRightFoot);

        CalculateFoot(AvatarIKGoal.LeftFoot, isLeftFootDown, hitLeftFoot, leftFootPositionWeight, leftFootRotationWeight);
        CalculateFoot(AvatarIKGoal.RightFoot, isRightFootDown, hitRightFoot, rightFootPositionWeight, rightFootRotationWeight);

    }


    private void CalculateFoot(AvatarIKGoal avatarIKGoal, bool isFootDown, RaycastHit raycastHit, float footPositionWeight, float footRotationWeight)
    {
        if (isFootDown)
        {
            animator.SetIKPositionWeight(avatarIKGoal, footPositionWeight);
            animator.SetIKPosition(avatarIKGoal, raycastHit.point + footOffset);

            Quaternion foodRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, raycastHit.normal), raycastHit.normal);
            animator.SetIKRotationWeight(avatarIKGoal, footRotationWeight);
            animator.SetIKRotation(avatarIKGoal, foodRotation);
        }
        else
        {
            animator.SetIKPositionWeight(avatarIKGoal, 0);
        }
    }
}