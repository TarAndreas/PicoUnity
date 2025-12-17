using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AvatarManager))]
public class AvatarManager_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AvatarManager avatarManager = (AvatarManager)target;
        if (GUILayout.Button("Load Default Avatar"))
        {
            avatarManager.LoadPersonalAvatar("", avatarManager.PersonalAvatarURL);
        }
    }
}

