using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;

public class ListVideos : MonoBehaviour
{
    public Button button;
    public GameObject menu;
    public GameObject handMenu;
    public GameObject control;
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
        videoNames = Directory.GetFiles(path+"/media/"+username, "*.mp4", SearchOption.AllDirectories);
        count = videoNames.Length;
       
        if (count!=0)
        {
            int i = 0;
            Button[] listButton = new Button[count];
            foreach (string name in videoNames)
            {
                Debug.Log(name);
                Button buttonPrefab = Instantiate(button, this.transform);
                listButton[i] = buttonPrefab;
                buttonPrefab.onClick.AddListener(() => ButtonClick(name));
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
    public void ButtonClick(string name)
    {
        menu.SetActive(false);
        handMenu.SetActive(true);
        control.GetComponent<ControlVideo>().StartVideo(name);
    } 
    
}
