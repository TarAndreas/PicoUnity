using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuplex.WebView; // Vuplex WebView plugin for web content in Unity
using UnityEngine.UI;

/// <summary>
/// Manages webview interactions in Unity, specifically the URL input and virtual keyboard display,
/// for immersive browsers or web panels using the Vuplex WebView package.
/// </summary>
public class WebViewInputURL : MonoBehaviour
{
    // Reference to the Vuplex CanvasWebViewPrefab component, which renders the WebView on a Canvas
    public CanvasWebViewPrefab webViewPrefab;

    // Reference to the UI input field where the user types the desired web address
    public InputField inputField;

    // Reference to the on-screen (virtual) keyboard used to enter text, separate from hardware keyboard
    public CanvasKeyboard webKeyboard;

    // Holds the currently loaded URL, for state tracking and later use if needed
    private string currentUrl;

    /// <summary>
    /// Unity's Start method, called on the frame when this script is enabled.
    /// Initializes the WebView and the virtual keyboard, and sets up event handling for keyboard visibility.
    /// </summary>
    async void Start()
    {
        // Wait until the WebView and Keyboard are fully initialized before proceeding
        await webViewPrefab.WaitUntilInitialized();
        await webKeyboard.WaitUntilInitialized();

        // Hide the keyboard initially
        webKeyboard.gameObject.SetActive(false);

        // Subscribe to the input focus change event from the WebView
        // Shows or hides the virtual keyboard depending on whether a web input field (in the webview) is focused
        webViewPrefab.WebView.FocusedInputFieldChanged += (sender, eventArgs) =>
        {
            var shouldShowKeyboard = eventArgs.Type != FocusedInputFieldType.None;
            webKeyboard.gameObject.SetActive(shouldShowKeyboard);
        };
    }

    /// <summary>
    /// Loads the URL typed by the user into the WebView.
    /// Called, for example, by a UI button press ("Go" or "Load").
    /// </summary>
    public void ChangeUrl()
    {
        // Get the text from the input field and store it as the current URL
        currentUrl = inputField.text;

        // Prepend "https://" and load the URL in the WebView
        webViewPrefab.WebView.LoadUrl("https://" + currentUrl);
    }
}
