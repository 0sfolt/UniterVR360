using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Video;

public class ControlVideo : MonoBehaviour
{
    public GameObject[] objectToHide;
    public FadeCanvas fadeCanvas;
    public Material videoMaterial;
    public VideoPlayer videoPlayer;
    public float fadeDuration = 1.0f;

    private Material _skyMaterial;
    // Start is called before the first frame update
    void Start()
    {
        _skyMaterial = RenderSettings.skybox;
    }

    public void StartVideo(string url)
    {
        videoPlayer.url = url;
        StartCoroutine(FadeAndSwitchVideo(videoMaterial, videoPlayer.Play));
    }

    public void PauseVideo()
    {
        StartCoroutine(FadeAndSwitchVideo(_skyMaterial, videoPlayer.Pause));
    }

    private IEnumerator FadeAndSwitchVideo(Material targetMaterial, Action onCompleteAction)
    {
        fadeCanvas.QuickFadeIn();
        yield return new WaitForSeconds(fadeDuration);

        SetObjectsActive(targetMaterial.Equals(_skyMaterial));
        fadeCanvas.QuickFadeOut();

        RenderSettings.skybox = targetMaterial;
        onCompleteAction.Invoke();
    }

    private void SetObjectsActive(bool isActive)
    {
        foreach(GameObject obj in objectToHide)
        {
            obj.SetActive(isActive);
        }
    }
}
