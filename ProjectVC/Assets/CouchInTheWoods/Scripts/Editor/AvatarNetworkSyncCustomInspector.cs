using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AvatarNetworkSync))]
public class AvatarNetworkSyncCustomInspector : Editor
{
    private AvatarNetworkSync networkSync;

    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();

        if(networkSync == null)
            networkSync = (AvatarNetworkSync) target;

        if (GUILayout.Button("Load Avatar"))
            networkSync.LoadAvatar(networkSync.myID, networkSync.myURL);

    }
}
