using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class AvatarMeshUpdater : MonoBehaviour
{
    /// <summary>
    /// Defines wehter the armature should be replaced by the newly downloaded avatar or not
    /// </summary>
    public bool shouldArmatureStay;

    [Header("Renderer References")]
    [SerializeField] List<SkinnedMeshRenderer> renderers = new List<SkinnedMeshRenderer>();

    public List<GameObject> meshParts;
    public List<string> localInvisibleBodyParts;

    PhotonView photonView;

    /// <summary>
    /// Defines wether the avatar should be displayed as first person or not
    /// </summary>
    public bool disableLocalAvatar;

    private void Start()
    {
        photonView = GetComponentInParent<PhotonView>();
    }

    public void SetNewAvatar(GameObject avatarObj)
    {
        for (int i = meshParts.Count - 1; i >= 0; i--)
            Destroy(meshParts[i]);

        if (meshParts == null)
            meshParts = new List<GameObject>();

        meshParts.Clear();

        int initialChildCount = avatarObj.transform.childCount;

        for (int i = avatarObj.transform.childCount - 1; i >= 0; i--)
        {
            Transform child = avatarObj.transform.GetChild(i);

            child.transform.SetParent(transform);
            child.transform.localPosition = Vector3.zero;
            child.transform.localRotation = Quaternion.identity;

            child.SetSiblingIndex(0);

            if (photonView.IsMine && localInvisibleBodyParts.Contains(child.name))
                child.GetComponent<SkinnedMeshRenderer>().enabled = false;

            meshParts.Add(child.gameObject);

            if (photonView.IsMine && disableLocalAvatar)
                child.gameObject.layer = LayerMask.NameToLayer("Character");
        }
    }

    public void BuildNewAvatar(GameObject avatarObj)
    {
        foreach (var renderer in renderers)
        {
            renderer.enabled = false;

            foreach (var newRenderer in avatarObj.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                if (newRenderer.name == renderer.name)
                {
                    renderer.enabled = true;

                    renderer.sharedMesh = newRenderer.sharedMesh;
                    renderer.material = newRenderer.material;
                }
            }
        }
    }

    public void GetNewAvatarData()
    {
        foreach (var newRenderer in AvatarManager.Instance.personalDownloadedAvatar.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            foreach (var renderer in renderers)
            {
                if (newRenderer.name == renderer.name)
                {
                    renderer.sharedMesh = newRenderer.sharedMesh;
                    renderer.material = newRenderer.material;
                }
            }
        }
    }
}
