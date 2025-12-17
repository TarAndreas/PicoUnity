using Photon.Pun;
using ReadyPlayerMe;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public struct CustomAvatarCompletionArgs
{
    public CompletionEventArgs Completion;
    public string userID;
}

public class AvatarManager : MonoBehaviour
{
    public static AvatarManager Instance { get; private set; }
    
    /// <summary>
    /// This is the ID for the chosen default avatar if no custom avatar has been created
    /// </summary>
    public int defaultAvatarID;

    /// <summary>
    /// List of default preview Avatars which are shown in the lobby to chose from
    /// </summary>
    public List<GameObject> defaultPreviewAvatars;

    [Header("Avatar")]
    /// <summary>
    /// This is the URL which represents the link to the users personal Avatar
    /// </summary>
    [SerializeField] private string personalAvatarURL;
    public string atlasAvatarURLPrefix = "?textureAtlas=none";
    public string PersonalAvatarURL { get => personalAvatarURL + atlasAvatarURLPrefix; }

    /// <summary>
    /// This object holds the downloaded Avatar
    /// </summary>
    [SerializeField] public GameObject personalDownloadedAvatar;

    /// <summary>
    /// This event gets thrown when an Avatar was downloaded successfully
    /// </summary>
    public UnityEvent<GameObject> AvatarLoadedEvent = new UnityEvent<GameObject>();

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    /// <summary>
    /// Downloads the Avatar with the Ready Player Me Avatar Manager and sends callbacks
    /// </summary>
    /// <param name="avatarURL"></param>
    /// <param name="OnCompleted"></param>
    /// <param name="OnFailed"></param>
    public void LoadAvatar(string userID, string avatarURL, EventHandler<CustomAvatarCompletionArgs> OnCompleted, EventHandler<FailureEventArgs> OnFailed)
    {
        AvatarLoader avatarLoader = new AvatarLoader();
        avatarLoader.OnCompleted += delegate (object sender, CompletionEventArgs e)
        {
            CustomAvatarCompletionArgs args = new CustomAvatarCompletionArgs();
            args.userID = userID;
            args.Completion = e;

            OnCompleted.Invoke(sender, args);
        };
        
        //avatarLoader.OnCompleted += (CompletionEventArgs e) => OnCompleted2;
        avatarLoader.OnFailed += OnFailed;
        avatarLoader.LoadAvatar(avatarURL);
    }

    /// <summary>
    /// Downloads the Avatar with the Ready Player Me Avatar Manager  
    /// </summary>
    /// <param name="avatarURL"></param>
    public void LoadPersonalAvatar(string userID, string avatarURL)
    {
        LoadAvatar(userID, avatarURL, PersonalAvatarLoadComplete, AvatarLoadFail);
    }

    /// <summary>
    /// Downloads the Avatar with the Ready Player Me Avatar Manager with the current provided URL
    /// </summary>
    public void LoadAvatar()
    {
        if(PhotonNetwork.LocalPlayer != null)
            LoadPersonalAvatar(PhotonNetwork.LocalPlayer.UserId, PersonalAvatarURL);
    }

    /// <summary>
    /// Sets the users Avatar download URL
    /// </summary>
    /// <param name="newURL"></param>
    public void UpdateAvatarURL(string newURL)
    {
        personalAvatarURL = newURL;
    }

    /// <summary>
    /// Callback for finished downloading the Avatar
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void PersonalAvatarLoadComplete(object sender, CustomAvatarCompletionArgs args)
    {
        personalDownloadedAvatar = args.Completion.Avatar.GameObject();

        Debug.Log($"Avatar loaded");
        personalDownloadedAvatar.SetActive(false);

        AvatarLoadedEvent.Invoke(personalDownloadedAvatar);
    }

    /// <summary>
    /// On Avatar download failed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void AvatarLoadFail(object sender, FailureEventArgs args)
    {
        Debug.Log($"Avatar loading failed with error message: {args.Message}");
    }

    /// <summary>
    /// Set default Avatar ID
    /// </summary>
    /// <param name="v"></param>
    internal void SetDefaultAvatarID(int v)
    {
        defaultAvatarID = v;
    }
}

