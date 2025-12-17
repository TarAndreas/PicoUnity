using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using Vuplex.WebView;


public class AvatarPreviewSelector : MonoBehaviour
{
    public static AvatarPreviewSelector Instance;

    /// <summary>
    /// Defines a list of transform points where to spawn the default avatars
    /// </summary>
    public List<Transform> spawnPoints;

    /// <summary>
    /// If true, spawn avatars on the defined Spawnpoints. Else spawn with automatic positions
    /// </summary>
    public bool spawnOnFixSpawnpoints;

    /// <summary>
    /// The avatar scale
    /// </summary>
    public float avatarScale;

    /// <summary>
    /// Distance between avatars when automatic placement 
    /// </summary>
    public float avatarDistance;

    /// <summary>
    /// Transform of the player avatar camera
    /// </summary>
    public Transform playerAvatar;

    /// <summary>
    /// Holds the current hovered avatar
    /// </summary>
    private Transform currentHoveredAvatar;

    /// <summary>
    /// Holds the current selected avatar
    /// </summary>
    private Transform currentSelectedAvatar;

    private List<Transform> previewAvatars;

    /// <summary>
    /// Color for hovered avatars
    /// </summary>
    public Color hoveredCharacterColor;
    /// <summary>
    /// Color for selected avatars
    /// </summary>
    public Color selectedCharacterColor;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CreateAvatarPreview();

        int rndID = UnityEngine.Random.Range(0, AvatarManager.Instance.defaultPreviewAvatars.Count);

        ActivateEventArgs activateEventArgs = new ActivateEventArgs();
        activateEventArgs.interactableObject = previewAvatars[rndID].GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();

        SetPreviewAvatarByID(rndID);
        OnAvatarClicked(activateEventArgs);
    }

    /// <summary>
    /// Instantiated the default Acatars on their spawn positions
    /// </summary>
    public void CreateAvatarPreview()
    {
        previewAvatars = new List<Transform>();

        float newAvatarXPos = 0;

        for (int i = 0; i < AvatarManager.Instance.defaultPreviewAvatars.Count; i++)
        {
            float newAvatarDist = 0;

            if(i % 2 == 1)
            {
                newAvatarDist = i * avatarDistance;
            } else
            {
                newAvatarDist = -i * avatarDistance;
            }

            newAvatarXPos += newAvatarDist;

            GameObject previewAvatar = Instantiate(AvatarManager.Instance.defaultPreviewAvatars[i], transform);

            previewAvatar.SetActive(true);

            TriggerInteractor interactor = previewAvatar.AddComponent<TriggerInteractor>();
            interactor.id = i;

            UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable interactable = previewAvatar.AddComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
            interactable.hoverEntered.AddListener(OnAvtarHovered);
            interactable.hoverExited.AddListener(OnAvatarHoverExited);
            interactable.activated.AddListener(OnAvatarClicked);

            Outline outline = previewAvatar.AddComponent<Outline>();
            outline.OutlineWidth = 3;
            outline.enabled = false;

            previewAvatar.transform.localScale = Vector3.one * avatarScale;

            if (!spawnOnFixSpawnpoints)
                previewAvatar.transform.localPosition = new Vector3(newAvatarXPos, 0, 0);
            else
                previewAvatar.transform.SetPositionAndRotation(spawnPoints[i].localPosition, spawnPoints[i].localRotation);

            previewAvatars.Add(previewAvatar.transform);

            previewAvatar.transform.SetSiblingIndex(i);
        }
    }

    private void Update()
    {
        if (playerAvatar == null)
            return;

        //Let the avatars always look towards player position
        for(int i = 0; i < previewAvatars.Count; i++)
        {
            Vector3 relativePlayerPos = new Vector3(playerAvatar.position.x, 0, playerAvatar.position.z) - new Vector3(previewAvatars[i].position.x, 0, previewAvatars[i].position.z);

            previewAvatars[i].rotation = Quaternion.LookRotation(relativePlayerPos, Vector3.up);
            previewAvatars[i].position = spawnPoints[i].position;
        }
    }

    /// <summary>
    /// On Avatar Hovered. Create outline color and set big preview avatar
    /// </summary>
    /// <param name="args"></param>
    public void OnAvtarHovered(HoverEnterEventArgs args)
    {
        if (currentHoveredAvatar != null && currentHoveredAvatar != currentSelectedAvatar)
            currentHoveredAvatar.GetComponent<Outline>().enabled = false;

        currentHoveredAvatar = args.interactableObject.transform;

        Outline outline = currentHoveredAvatar.GetComponent<Outline>();
        outline.enabled = true;
        outline.OutlineColor = hoveredCharacterColor;
        
        TriggerInteractor interactor = currentHoveredAvatar.GetComponent<TriggerInteractor>();

        SetPreviewAvatarByID(interactor.id);
    }


    /// <summary>
    /// Destroy outline and revert big preview avatar to last.
    /// </summary>
    /// <param name="args"></param>
    public void OnAvatarHoverExited(HoverExitEventArgs args)
    {
        if (currentHoveredAvatar != null)
            currentHoveredAvatar.GetComponent<Outline>().enabled = false;

        if(currentSelectedAvatar != null)
        {
            Outline outline = currentSelectedAvatar.GetComponent<Outline>();
            outline.enabled = true;
            outline.OutlineColor = selectedCharacterColor;
        }

        RPMAvatarWebviewLoader.Instance.RevertToActiveAvatar();
    }

    /// <summary>
    /// Change outline and lock big avatar preview
    /// </summary>
    /// <param name="args"></param>
    public void OnAvatarClicked(ActivateEventArgs args)
    {
        if (currentHoveredAvatar != null)
            currentHoveredAvatar.GetComponent<Outline>().enabled = false;

        if (currentSelectedAvatar != null)
            currentSelectedAvatar.GetComponent<Outline>().enabled = false;

        currentSelectedAvatar = args.interactableObject.transform;

        Outline outline = currentSelectedAvatar.GetComponent<Outline>();
        outline.enabled = true;
        outline.OutlineColor = selectedCharacterColor;

        TriggerInteractor interactor = currentSelectedAvatar.GetComponent<TriggerInteractor>();
        RPMAvatarWebviewLoader.Instance.SetActiveAvatar(AvatarManager.Instance.defaultPreviewAvatars[interactor.id]);
        AvatarManager.Instance.SetDefaultAvatarID(args.interactableObject.transform.GetSiblingIndex());
        AvatarManager.Instance.UpdateAvatarURL(String.Empty);
    }

    public void ClearOutlines()
    {
        if (currentHoveredAvatar != null)
            currentHoveredAvatar.GetComponent<Outline>().enabled = false;

        if (currentSelectedAvatar != null)
            currentSelectedAvatar.GetComponent<Outline>().enabled = false;

        currentHoveredAvatar = null;
        currentSelectedAvatar = null;
    }

    public void OnAvatarClicked(SelectEnterEventArgs args)
    {
        TriggerInteractor interactor = args.interactableObject.transform.GetComponent<TriggerInteractor>();
        RPMAvatarWebviewLoader.Instance.SetActiveAvatar(AvatarManager.Instance.defaultPreviewAvatars[interactor.id]);
    }

    private void SetPreviewAvatarByID(int id)
    {
        RPMAvatarWebviewLoader.Instance.SetAvatarToPreviewPosition(Instantiate(AvatarManager.Instance.defaultPreviewAvatars[id]).transform);
    }

}
