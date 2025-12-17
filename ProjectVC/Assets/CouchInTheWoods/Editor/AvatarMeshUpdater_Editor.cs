using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AvatarMeshUpdater))]
public class AvatarMeshUpdater_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        //AvatarMeshUpdater avatarManager = (AvatarMeshUpdater)target;
        //if (GUILayout.Button("Update Mesh Renderers"))
        //{
        //    //avatarManager.();
        //}

    }
}
