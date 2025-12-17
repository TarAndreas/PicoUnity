using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class VRRigAvatarMapper : MonoBehaviour
{
    public List<BoneMapping> rigMapping;

    public MultiParentConstraint multiParentConstraint;
    public TwoBoneIKConstraint twoBoneIKConstraint_left;
    public TwoBoneIKConstraint twoBoneIKConstraint_right;

    public void MapRig(GameObject newAvatar)
    {
        foreach (BoneMapping mapping in rigMapping)
            MapBone(mapping, newAvatar);
    }

    private void MapBone(BoneMapping mapping, GameObject newAvatar)
    {
        mapping.bone = RecursiveFindChild(newAvatar.transform, mapping.name);

        switch(mapping.name)
        {
            case "Head":
                multiParentConstraint.data.constrainedObject = mapping.bone;
                break;
            case "LeftArm":
                twoBoneIKConstraint_left.data.root = mapping.bone;
                break;
            case "LeftForeArm":
                twoBoneIKConstraint_left.data.mid = mapping.bone;
                break;
            case "LeftHand":
                twoBoneIKConstraint_left.data.tip = mapping.bone;
                break;
            case "RightArm":
                twoBoneIKConstraint_right.data.root = mapping.bone;
                break;
            case "RightForeArm":
                twoBoneIKConstraint_right.data.mid = mapping.bone;
                break;
            case "RightHand":
                twoBoneIKConstraint_right.data.tip = mapping.bone;
                break;
        }
    }

    Transform RecursiveFindChild(Transform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName)
            {
                return child;
            }
            else
            {
                Transform found = RecursiveFindChild(child, childName);
                if (found != null)
                {
                    return found;
                }
            }
        }
        return null;
    }

    [Serializable]
    public class BoneMapping
    {
        public string name;
        public Transform bone;
    }

}
