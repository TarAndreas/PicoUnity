using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Terminates the executed application or program in Unity
/// </summary>
public class QuitApp : MonoBehaviour
{
    public void doQuit()
    {
        Debug.Log("Projekt wird beendet");
        Application.Quit();
    }
}
