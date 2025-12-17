using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AvatarController))]
public class AvatarController_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        //AvatarController avatarController = (AvatarController)target;
        //if (GUILayout.Button("Load Transforms to Renderer"))
        //{
        //    //avatarController.;
        //}
    }
}
