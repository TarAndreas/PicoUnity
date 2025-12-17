using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AppServerSettingOption : MonoBehaviour
{
    public AppServerSettingData data;

    string filePath;

    void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "appdata.txt");
    }

    public void Save()
    {
        List<string> lines = new List<string>();
        foreach (var pair in data.variables)
        {
            lines.Add($"{pair.Key}={pair.Value}");
        }
        File.WriteAllLines(filePath, lines);
        //Debug.Log($"String data saved to: {filePath}");
    }

    public void Load()
    {
        data.variables.Clear();
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                var split = line.Split('=');
                if (split.Length == 2)
                {
                    data.variables[split[0]] = split[1];
                }
            }
            Debug.Log("String data loaded.");
        }
        else
        {
            Debug.LogWarning("Save file not found.");
        }
    }
}
