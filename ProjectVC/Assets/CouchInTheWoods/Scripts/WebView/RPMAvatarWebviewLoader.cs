//#if VUPLEX_CCU
using UnityEngine;
using ReadyPlayerMe;
using Vuplex.WebView;
using UnityEngine.Events;
using UnityEngine.UI;

public class RPMAvatarWebviewLoader : MonoBehaviour
{
    public static RPMAvatarWebviewLoader Instance;

    public Transform avaterPreviewHolder;

    private GameObject avatar;
    private AvatarLoader avatarLoader;
    private VuplexWebView vuplexWebView;

    public Button characterCreationButton;

    [SerializeField] private GameObject loading;
    [SerializeField] private BaseWebViewPrefab canvasWebView;

    public UnityEvent OnAvatarLoadedSuccessfully;

    private GameObject activeAvatar;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        vuplexWebView = new VuplexWebView();
        vuplexWebView.Initialize(canvasWebView);
        vuplexWebView.OnInitialized = () => Debug.Log("WebView Initialized");
        vuplexWebView.OnAvatarUrlReceived = OnAvatarUrlReceived;

#if PLATFORM_ANDROID
        characterCreationButton.interactable = false;
#endif
    }

    // WebView callback for retrieving avatar url
    private void OnAvatarUrlReceived(string avatarUrl)
    {
        loading.SetActive(true);
        avatarLoader = new AvatarLoader();
        avatarLoader.OnCompleted += OnAvatarLoadCompleted;
        avatarLoader.LoadAvatar(avatarUrl);

        AvatarPreviewSelector.Instance.ClearOutlines();

        AvatarManager.Instance.UpdateAvatarURL(avatarUrl);
    }

    private void OnAvatarLoadCompleted(object sender, CompletionEventArgs args)
    {
        if (avatar != null)
            Destroy(avatar);

        loading.SetActive(false);
        //SetWebViewVisibility(false);

        SetAvatarToPreviewPosition(args.Avatar.transform);
        SetActiveAvatar(args.Avatar);

        OnAvatarLoadedSuccessfully?.Invoke();
    }

    public void SetWebViewVisibility(bool visible)
    {
        canvasWebView.gameObject.SetActive(visible);
    }

    public void SetAvatarToPreviewPosition(Transform _avatar)
    {
        if (avatar != null)
            Destroy(avatar);

        if (_avatar.gameObject != activeAvatar)
            HideActiveAvatar();
        //else
        //    activeAvatar.SetActive(true);

        avatar = _avatar.gameObject;

        //avatar.transform.Rotate(Vector3.up, 180);
        //avatar.transform.position = new Vector3(0.75f, 0, 0.75f);
        avatar.gameObject.SetActive(true);

        avatar.transform.SetPositionAndRotation(avaterPreviewHolder.position, avaterPreviewHolder.rotation);
    }

    private void HideActiveAvatar()
    {
        if (activeAvatar != null)
            activeAvatar.SetActive(false);
    }

    public void RevertToActiveAvatar()
    {
        if (activeAvatar == null)
            return;

        SetAvatarToPreviewPosition(Instantiate(activeAvatar).transform);
    }

    public void SetActiveAvatar(GameObject _avatar)
    {
        if (activeAvatar != null)
            Destroy(activeAvatar);

        activeAvatar = Instantiate(_avatar);
        HideActiveAvatar();

        AvatarManager.Instance.personalDownloadedAvatar = activeAvatar;
        DontDestroyOnLoad(activeAvatar);
    }

    public void OnDestroy()
    {
        if (avatar != null)
            Destroy(avatar);
    }

}
//#endif
