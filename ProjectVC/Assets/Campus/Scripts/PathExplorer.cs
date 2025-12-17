using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using Vuplex.WebView;

public class PathExplorer : MonoBehaviour
{
    string path;
    //public RawImage image;

    public CanvasWebViewPrefab canvasWebViewPrefab;

    public InputField WebInputField;

    async void Start()
    {
        Web.SetUserAgent(false);

        canvasWebViewPrefab = GameObject.Find("CanvasWebViewPrefab").GetComponent<CanvasWebViewPrefab>();
        initCanvasKeyboard();

        await canvasWebViewPrefab.WaitUntilInitialized();
    }



#if UNITY_EDITOR
    public void OpenExplorer()
    {

        path = EditorUtility.OpenFilePanel("Overwrite Path", "", "png");
        Debug.Log(path);

        //GetImage();

        LoadFromPath();
    }
#endif

    /*
    void GetImage()
    {
        if (path != null)
        {
            UpdateImage();
        }
    }

    void UpdateImage()
    {
        WWW www = new WWW("file://" + path);
        image.texture = www.texture;
    }    
    */


    async void LoadFromPath() {

        canvasWebViewPrefab = GameObject.Find("CanvasWebViewPrefab").GetComponent<CanvasWebViewPrefab>();
        await canvasWebViewPrefab.WaitUntilInitialized();

        //var filePath = $"{Application.persistentDataPath}/myfile.html";
        // Spaces in URLs must be escaped
        //var fileUrl = "file://" + filePath.Replace(" ", "%20");

        var fileUrl = "file://" + path;
        canvasWebViewPrefab.WebView.LoadUrl(fileUrl);
      }

    public async void HomeButton()
    {
        canvasWebViewPrefab = GameObject.Find("CanvasWebViewPrefab").GetComponent<CanvasWebViewPrefab>();
        await canvasWebViewPrefab.WaitUntilInitialized();

        var homeUrl = "https://www.google.de";
        canvasWebViewPrefab.WebView.LoadUrl(homeUrl);
    }
    public async void GoButton()
    {
        canvasWebViewPrefab = GameObject.Find("CanvasWebViewPrefab").GetComponent<CanvasWebViewPrefab>();
        await canvasWebViewPrefab.WaitUntilInitialized();

        var InputUrl = WebInputField.text;
        canvasWebViewPrefab.WebView.LoadUrl(InputUrl);
    }
    public void CopyButton()
    {
        TextEditor _textEditor = new TextEditor();
        _textEditor.text = WebInputField.text;
        _textEditor.SelectAll();
        _textEditor.Copy();
    }
    public void PasteButton()
    {
        TextEditor _textEditor = new TextEditor();
        _textEditor.Paste();
        WebInputField.text = _textEditor.text;
    }

    public async void FocusInputField()
    {
        await canvasWebViewPrefab.WaitUntilInitialized();
        canvasWebViewPrefab.WebView.FocusedInputFieldChanged += (sender, eventArgs) => {
            Debug.Log("Focused input field changed. Text input is focused: ");
        };
    }

    public void initCanvasKeyboard()
    {
        // Also hook up the on-screen keyboard.
        var keyboard = GameObject.FindObjectOfType<CanvasKeyboard>();
        keyboard.InputReceived += (sender, eventArgs) =>
        {
            canvasWebViewPrefab.WebView.SendKey(eventArgs.Value);
        };
    }

    public async void LoadURLFromInputField()
    {
        canvasWebViewPrefab = GameObject.Find("CanvasWebViewPrefab").GetComponent<CanvasWebViewPrefab>();
        await canvasWebViewPrefab.WaitUntilInitialized();

        //var myURL = "https://" + WebInputField.text;

        //canvasWebViewPrefab.WebView.LoadUrl(myURL);
        //canvasWebViewPrefab.WebView.LoadUrl("https://" + WebInputField.text);

        Debug.Log("Inputfield URL: " + WebInputField.text);
    }
    


}
