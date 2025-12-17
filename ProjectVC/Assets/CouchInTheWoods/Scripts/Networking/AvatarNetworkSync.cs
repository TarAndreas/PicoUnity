using Photon.Pun;
using ReadyPlayerMe;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.Events;
using Photon.Realtime;

/// <summary>
/// Handles all RPM avatar related stuff which gets synchronized between remotes
/// </summary>
public class AvatarNetworkSync : MonoBehaviourPun, IPunObservable
{
    /// <summary>
    /// Holds the avatarURL of all other users which already got downloaded
    /// </summary>
    private Dictionary<string, string> otherPlayerAvatarsURLs;
    private Dictionary<string, GameObject> otherPlayerAvatarObjects;

    /// <summary>
    /// This is the local player's Photon ID
    /// </summary>
    public string myID;
    /// <summary>
    /// If the player has a default avatar, this is the ID
    /// </summary>
    public int myDefaultAvatarID;
    /// <summary>
    /// The RPM avatar URL created by the user
    /// </summary>
    public string myURL;

    private float originalTurnSmoothness;

    /// <summary>
    /// Holds the state if the default avatar was already shown
    /// </summary>
    private bool defaultAvatarLoaded;

    public UnityEvent<GameObject> OnAvatarLoaded;

    private AvatarSeatController seatController;

    public AvatarInputConverter inputConverter;

    private AvatarController avatarController;

    private LowerBodyAnimation lowerBodyAnimation;

    public bool isSitting;

    //Default values for sitting
    public Vector3 raycastLeftFootSitting = new Vector3(-0.3f, 0, 0.1f);
    public Vector3 raycastRightFootSitting = new Vector3(-0.3f, 0, -0.1f);
    public Vector3 raycastLeftFootDefault= new Vector3(0, 0.7f, 0);
    public Vector3 raycastRightFootDefault= new Vector3(0, 0.7f, 0);

    public Vector3 offsetRotation;

    /// <summary>
    /// The duration the avatar still gets synced after started sitting on a chair, before position gets ultimately locked to the chair
    /// </summary>
    public float syncTime;

    Vector3 seatPosition;
    Quaternion seatRotation;

    public GameObject leftFirstPersonController;
    public GameObject rightFirstPersonController;

    private void Awake()
    {
        otherPlayerAvatarsURLs = new Dictionary<string, string>();
        otherPlayerAvatarObjects = new Dictionary<string, GameObject>();
        lowerBodyAnimation = GetComponent<LowerBodyAnimation>();
        avatarController = GetComponent<AvatarController>();
    }

    private void Start()
    {
        if (!PhotonNetwork.InRoom)
            Destroy(this);

        originalTurnSmoothness = avatarController.turnSmoothness;

        seatController = GetComponent<AvatarSeatController>();

        //Load the local avatar if this is the local player
        if (PhotonView.Get(this).IsMine)
        {
            myURL = AvatarManager.Instance.PersonalAvatarURL;
            myID = PhotonNetwork.LocalPlayer.UserId;
            myDefaultAvatarID = AvatarManager.Instance.defaultAvatarID;

            LoadLocalAvatar();
        } else
        {
            seatController.avatarSeatPositionOffset.y = -0.9f;
            leftFirstPersonController.gameObject.SetActive(false);
            rightFirstPersonController.gameObject.SetActive(false);
        }

        ConnectToNetwork.OnPlayerLeft.AddListener(CleanupPlayer);
    }


    /// <summary>
    /// Syncs states between the remotes for handling the ARPM avatars
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="info"></param>
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(myID);
            stream.SendNext(myURL);
            stream.SendNext(myDefaultAvatarID);
            stream.SendNext(isSitting);

            Vector3 position = seatController.seatTarget_transform != null ? seatController.seatTarget_transform.position : Vector3.zero;
            Quaternion rotation = seatController.seatTarget_transform != null ? seatController.seatTarget_transform.rotation : Quaternion.identity;

            stream.SendNext(position);
            stream.SendNext(rotation);
        }
        else
        {
            // Network player, receive data
            string photonID = (string)stream.ReceiveNext();
            string avatarURL = (string)stream.ReceiveNext();
            int otherDefaultAvatarID = (int)stream.ReceiveNext();
            bool isSittingTemp = (bool)stream.ReceiveNext();
            seatPosition = (Vector3)stream.ReceiveNext();
            seatRotation = (Quaternion)stream.ReceiveNext();

            //Loads the default avatar in advance (before the real avatar [if existent] gets loaded
            if (!defaultAvatarLoaded)
                LoadDefaultAvatar(otherDefaultAvatarID);
            else
                LoadAvatar(photonID, avatarURL);

            UpdateSittingOffset(isSittingTemp);

            if(isSitting)
                seatController.SetPositions(seatPosition, seatRotation);
        }
    }

    //Updates some offsets required for letting the avatar sit
    private void UpdateSittingOffset(bool sitting)
    {
        if (isSitting == sitting)
            return;

        isSitting = sitting;

        if (isSitting)
        {
            inputConverter.head_positionMin = new Vector3(0, 1, 0);

            Vector3 leftFootOffset = (transform.localRotation * Quaternion.Euler(offsetRotation)) * raycastLeftFootSitting;
            Vector3 rightFootOffset = (transform.localRotation * Quaternion.Euler(offsetRotation)) * raycastRightFootSitting;

            lowerBodyAnimation.raycastLeftOffset = new Vector3(leftFootOffset.x, raycastLeftFootDefault.y, leftFootOffset.z);
            lowerBodyAnimation.raycastRightOffset = new Vector3(rightFootOffset.x, raycastRightFootDefault.y, rightFootOffset.z);
            avatarController.turnSmoothness = 0;

            seatController.isAvatarSeat = true;

            Invoke("RemoteBodySyncOff", syncTime);
        }
        else
        {
            inputConverter.head_positionMin = new Vector3(0, 1.7f, 0);
            lowerBodyAnimation.raycastLeftOffset = raycastLeftFootDefault;
            lowerBodyAnimation.raycastRightOffset = raycastRightFootDefault;

            avatarController.turnSmoothness = originalTurnSmoothness;
            seatController.isAvatarSeat = false;

            GetComponentInParent<MultiplayerVRSynchronization>().remoteSyncBody = true;
        }        
    }

    private void RemoteBodySyncOff()
    {
        GetComponentInParent<MultiplayerVRSynchronization>().remoteSyncBody = false;
    }

    public void SetSittingState(bool state)
    {
        UpdateSittingOffset(state);
    }

    /// <summary>
    /// Loads the avatar via URL for remote players
    /// </summary>
    /// <param name="photonViewID"></param>
    /// <param name="avatarURL"></param>
    public void LoadAvatar(string photonViewID, string avatarURL)
    {
        if (otherPlayerAvatarsURLs.ContainsKey(photonViewID))
            return;

        otherPlayerAvatarsURLs.Add(photonViewID, avatarURL);
        otherPlayerAvatarObjects.Add(photonViewID, null);

        Debug.Log("Sync Avatar: " + photonViewID + " -> " + avatarURL);

        AvatarManager.Instance.LoadAvatar(photonViewID, avatarURL, AvatarLoadComplete, AvatarLoadFail);
    }

    /// <summary>
    /// Displays the local avatar (default or already downladed custom avatar).
    /// </summary>
    private void LoadLocalAvatar()
    {
        AvatarLoadComplete(AvatarManager.Instance.personalDownloadedAvatar);
    }

    /// <summary>
    /// Load the selected default avatar
    /// </summary>
    /// <param name="id"></param>
    public void LoadDefaultAvatar(int id)
    {
        if (defaultAvatarLoaded)
            return;

        defaultAvatarLoaded = true;

        AvatarLoadComplete(Instantiate(AvatarManager.Instance.defaultPreviewAvatars[id]));
    }

    private void AvatarLoadComplete(object sender, CustomAvatarCompletionArgs args)
    {
        if (!otherPlayerAvatarObjects.ContainsKey(args.userID))
            otherPlayerAvatarObjects.Add(args.userID, args.Completion.Avatar);
        else
            otherPlayerAvatarObjects[args.userID] = args.Completion.Avatar;

        AvatarLoadComplete(args.Completion.Avatar.GameObject());
    }

    private void AvatarLoadComplete(GameObject avatar)
    {
        Debug.Log($"Avatar loaded 2");

        StartCoroutine(AvatarActivationRoutine(avatar));
    }

    /// <summary>
    /// This routine maps the downloaded avatar to the rig, xrRig and animator to get it fully functional
    /// </summary>
    /// <returns></returns>
    private IEnumerator AvatarActivationRoutine(GameObject avatar)
    {
        Animator animator = GetComponent<Animator>();

        animator.enabled = false;

        yield return null;

        avatar.SetActive(true);

        Transform armature = avatar.transform.Find("Armature");

        if (armature == null)
        {
            GameObject armatureObj = new GameObject("Armature");
            armatureObj.transform.SetParent(avatar.transform);
            armatureObj.transform.localPosition = Vector3.zero;
            armatureObj.transform.localRotation = Quaternion.identity;

            Transform hips = avatar.transform.Find("Hips");

            if (hips != null)
                hips.SetParent(armatureObj.transform, true);
        }

        OnAvatarLoaded?.Invoke(avatar);

        yield return new WaitForEndOfFrame();

        animator.Rebind();
        animator.Update(0f);

        yield return new WaitForEndOfFrame();

        animator.enabled = true;

        animator.Rebind();
        animator.Update(0f);

        Debug.Log("downloaded Avatar " + avatar.name);

        if (avatar != null)
            Destroy(avatar);
    }

    private void AvatarLoadFail(object sender, FailureEventArgs args)
    {
        Debug.Log($"Avatar loading failed with error message: {args.Message}");
    }

    private void CleanupPlayer(Player player)
    {
        if (otherPlayerAvatarsURLs.ContainsKey(player.UserId))
        {
            otherPlayerAvatarsURLs.Remove(player.UserId);
        }

        if (otherPlayerAvatarObjects.ContainsKey(player.UserId))
        {
            if (otherPlayerAvatarObjects[player.UserId] != null)
                Destroy(otherPlayerAvatarObjects[player.UserId]);

            otherPlayerAvatarObjects.Remove(player.UserId);
        }
    }
}
