using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;
using System.Text.RegularExpressions;
using System.Linq;
using System;
public class ListVideos : MonoBehaviour
{
    public Button button;
    public GameObject menu;
    public GameObject handMenu;
    public GameObject control;
    public Button matin, midi, soir;
    private string[] listButtonName;
    private string path;
    public TMP_InputField usernameField;
    private string username;
    private string[] videoNames;
    private int count=0;
    public void StartList()
    {
        DestroyAllButtonsInParent(this.transform);
        username = usernameField.text;
        path = Application.persistentDataPath;
        if (!Directory.Exists(path+"/media"))
        {
            var folder = Directory.CreateDirectory(path + "/media");
        }
        string destinationPath = path + "/media/" + username;
        videoNames = Directory.GetFiles(destinationPath, "*.mp4", SearchOption.AllDirectories);
        count = videoNames.Length;
        if (count!=0)
        {
            foreach (string name in videoNames)
            {
                string shortName = Path.GetFileNameWithoutExtension(name);
                string extension = Path.GetExtension(name);

                string[] endings = { "soir", "matin", "midi" };
                string matchingEnding = endings.FirstOrDefault(e => shortName.EndsWith(e, StringComparison.OrdinalIgnoreCase));
                if (matchingEnding != null)
                {
                    // Remove the ending to get the common base name
                    string baseName = shortName.Substring(0, shortName.Length - matchingEnding.Length).TrimEnd('_', '-', ' ');

                    // Create a new folder based on the base name
                    string targetFolder = Path.Combine(destinationPath, baseName);
                    if (!Directory.Exists(targetFolder))
                    {
                        Directory.CreateDirectory(targetFolder);
                    }

                    // Move the file into the new folder
                    string destination = Path.Combine(targetFolder, shortName + extension);
                    Debug.Log(destination);
                    Debug.Log(File.Exists(destination));
                    if (!File.Exists(destination))
                    {
                        File.Move(name, destination);
                    }

                    Console.WriteLine($"Moved {shortName}{extension} to {targetFolder}");
                }

            }
            int i = 0;
            Button[] listButton = new Button[count];
            string[] directoryNames = Directory.GetDirectories(destinationPath);
            foreach (string dirName in directoryNames)
            {
                string shortName = Path.GetFileNameWithoutExtension(dirName);
                Button buttonPrefab = Instantiate(button, this.transform);
                TMP_Text buttonText = buttonPrefab.GetComponentInChildren<TMP_Text>();
                buttonText.text = shortName;
                listButton[i] = buttonPrefab;
                string firstVideo = Directory.GetFiles(dirName, "*.mp4",SearchOption.TopDirectoryOnly)[0];
                buttonPrefab.onClick.AddListener(() => ButtonClick(firstVideo,dirName,shortName));
                i += 1;
            }
        }
    }

    public void DestroyAllButtonsInParent(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }
    public void ButtonClick(string name, string dirName, string shortName)
    {
        handMenu.SetActive(true);
        control.GetComponent<ControlVideo>().StartVideo(name);
        if (File.Exists(dirName+"/"+shortName+"_matin.mp4"))
        {
            matin.GetComponent<SwitchVideo>().url = dirName + "/" + shortName + "_matin.mp4";
        }
        if (File.Exists(dirName + "/" + shortName + "_midi.mp4"))
        {
            SwitchVideo urlmidi = midi.GetComponent<SwitchVideo>();
            urlmidi.url = dirName + "/" + shortName + "_midi.mp4";
        }
        if (File.Exists(dirName + "/" + shortName + "_soir.mp4"))
        {
            soir.GetComponent<SwitchVideo>().url = dirName + "/" + shortName + "_soir.mp4";
        }
    } 
    
    public void PlayVideo(string period)
    {

    }
}
