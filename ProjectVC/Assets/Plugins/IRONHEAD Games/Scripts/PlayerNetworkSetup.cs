using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

using TMPro;

public class PlayerNetworkSetup : MonoBehaviourPunCallbacks
{
    public GameObject LocalXRRigGameObject;

    
    public GameObject MainAvatarGameobject;

    public GameObject AvatarHeadGameobject;
    public GameObject AvatarBodyGameobject;

    public GameObject[] AvatarModelPrefabs;

    public TextMeshProUGUI PlayerName_Text;

    // Start is called before the first frame update
    void Start()
    {
        //Setup the player
        if(photonView.IsMine)
        {
            LocalXRRigGameObject.SetActive(true);
            gameObject.GetComponent<MovementController>().enabled = true;
            gameObject.GetComponent<AvatarInputConverter>().enabled = true;

            //Getting the avatar selesction data so that the correct avatar models can be instantiated
            object avatarSelectionNumber;
            if( PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiplayerVRConstants.AVATAR_SELECTION_NUMBER, out avatarSelectionNumber ))
            {
                Debug.Log("Avatar selection number: " + (int) avatarSelectionNumber);
                photonView.RPC("InitializeSelectedAvatarModel", RpcTarget.AllBuffered, (int)avatarSelectionNumber );
            }
            

            SetLayerRecursively(AvatarHeadGameobject,11);
            SetLayerRecursively(AvatarBodyGameobject,12);

            UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationArea[] teleportationAreas = GameObject.FindObjectsOfType<UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationArea>();
            if(teleportationAreas.Length>0)
            {
                Debug.Log("Found " + teleportationAreas.Length + " teleportation areas.");
                foreach(var item in teleportationAreas)
                {
                    item.teleportationProvider = LocalXRRigGameObject.GetComponent<UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationProvider>();

                }
            }

            UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationAnchor[] teleportationAnchors = GameObject.FindObjectsOfType<UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationAnchor>();
            if (teleportationAnchors.Length > 0)
            {
                Debug.Log("Found " + teleportationAnchors.Length + " teleportation areas.");
                foreach (var item in teleportationAnchors)
                {
                    item.teleportationProvider = LocalXRRigGameObject.GetComponent<UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationProvider>();

                }
            }

            MainAvatarGameobject.AddComponent<AudioListener>();
        }
        else
        {
            //the Player is remote
            LocalXRRigGameObject.SetActive(false);
            gameObject.GetComponent<MovementController>().enabled = false;
            gameObject.GetComponent<AvatarInputConverter>().enabled = false;
            SetLayerRecursively(AvatarHeadGameobject,0);
            SetLayerRecursively(AvatarBodyGameobject,0);
        }

        if(PlayerName_Text.text != null && photonView != null && photonView.Owner != null)
        {
            PlayerName_Text.text = photonView.Owner.NickName;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetLayerRecursively(GameObject go, int layerNumber)
    {
        if (go == null) return;
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layerNumber;
        }
    }

    [PunRPC]
    public void InitializeSelectedAvatarModel(int avatarSelectionNumber)
    {
        GameObject selectedAvatarGameobject = Instantiate(AvatarModelPrefabs[avatarSelectionNumber],LocalXRRigGameObject.transform);

        AvatarInputConverter avatarInputConverter = transform.GetComponent<AvatarInputConverter>();
        AvatarHolder avatarHolder = selectedAvatarGameobject.GetComponent<AvatarHolder>();
        SetUpAvatarGameobject(avatarHolder.HeadTransform,avatarInputConverter.AvatarHead);
        SetUpAvatarGameobject(avatarHolder.BodyTransform,avatarInputConverter.AvatarBody);
        SetUpAvatarGameobject(avatarHolder.HandLeftTransform, avatarInputConverter.AvatarHand_Left);
        SetUpAvatarGameobject(avatarHolder.HandRightTransform, avatarInputConverter.AvatarHand_Right);
    }

    void SetUpAvatarGameobject(Transform avatarModelTransform, Transform mainAvatarTransform)
    {
        avatarModelTransform.SetParent(mainAvatarTransform);
        avatarModelTransform.localPosition = Vector3.zero;
        avatarModelTransform.localRotation = Quaternion.identity;
    }
}
