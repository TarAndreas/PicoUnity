using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonRemotePlayerComponentsHandler : MonoBehaviour
{
    public List<Component> onlyLocalPlayerComponents;
    public List<GameObject> enableIfLocalObjects;
    private void Awake()
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            foreach(GameObject obj in enableIfLocalObjects)
            {
                obj.SetActive(true);
            }
            Destroy(this);
            return;
        }

        for (int i = onlyLocalPlayerComponents.Count - 1; i >= 0; i--)
            Destroy(onlyLocalPlayerComponents[i]);
    }
}
