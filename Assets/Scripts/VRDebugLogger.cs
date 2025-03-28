using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VRDebugLogger : MonoBehaviour
{
    Dictionary<string, string> debugLogs = new Dictionary<string, string>();

    public TextMeshProUGUI display;
    private void Update()
    {
        Debug.Log("time:" + Time.time);
        Debug.Log(gameObject.name);
    }
    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        
        string[] splitString = logString.Split(char.Parse(":"));
        string debugKey = splitString[0];
        string debugValue = splitString.Length > 1 ? splitString[1] : "";

        if (debugLogs.ContainsKey(debugKey))
        {
            debugLogs[debugKey] = debugValue;
        }
        else
        {
            debugLogs.Add(debugKey, debugValue);
        }
        
        string displayText = "";
        foreach (KeyValuePair<string,string> log in debugLogs)
        {
            if (log.Value=="")
            {
                displayText += log.Key + "\n";
            }
            else
            {
                displayText += log.Key + ": " + log.Value + "\n";
            }
        }
        display.text = displayText;
    }
}
