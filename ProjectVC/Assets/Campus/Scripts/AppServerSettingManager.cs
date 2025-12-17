using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppServerSettingManager : MonoBehaviour
{
    public AppServerSettingOption appServerSettingOption;
    // Start is called before the first frame update
    void Start()
    {
        appServerSettingOption.data.variables["AppID"] = "A";
        appServerSettingOption.data.variables["VoiceID"] = "B";
        appServerSettingOption.data.variables["ChatID"] = "C";

        appServerSettingOption.Save();

        // Optionally clear and reload
        appServerSettingOption.data = new AppServerSettingData();
        appServerSettingOption.Load();
        foreach (var pair in appServerSettingOption.data.variables)
        {
            //Debug.Log($"{pair.Key}: {pair.Value}");
        }
    }


}
