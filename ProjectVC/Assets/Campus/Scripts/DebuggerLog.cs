/*
 * Create text file on path. Write log data from Unity to the text file.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DebuggerLog : MonoBehaviour
{

    string filename = "";

    private void OnEnable()
    {
        Application.logMessageReceived += Log; 
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= Log;
    }


    // Start is called before the first frame update
    void Awake()
    {
        filename = Application.dataPath + "/LogFile.text";
    }

    public void Log(string logString, string stackTrace, LogType type)
    {
        TextWriter tw = new StreamWriter(filename, true);
        tw.WriteLine("[" + System.DateTime.Now + "]" + logString);

        tw.Close();
    }


}
