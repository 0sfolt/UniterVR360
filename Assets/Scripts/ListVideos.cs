using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ListVideos : MonoBehaviour
{
    public Button button;
    public GameObject menu;
    public GameObject handMenu;
    public GameObject control;
    public string path ;
    private string[] videoNames;
    private int count=0;
    void Start()
    {
        videoNames = Directory.GetFiles(path, "*.mp4", SearchOption.AllDirectories);
        count = videoNames.Length;
        if (count!=0)
        {
            foreach (string name in videoNames)
            {
                Debug.Log(name);
                Button buttonPrefab = Instantiate(button, this.transform);
                buttonPrefab.onClick.AddListener(() => ButtonClick(name));
            }
        }
    }

    public void ButtonClick(string name)
    {
        menu.SetActive(false);
        handMenu.SetActive(true);
        Debug.Log(name);
        control.GetComponent<ControlVideo>().StartVideo(name);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
