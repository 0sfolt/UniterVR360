using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Video;

public class SwitchVideo : MonoBehaviour
{
    public FadeCanvas fadeCanvas;
    public Material videoMaterial;
    public VideoPlayer videoPlayer;
    public float fadeDuration = 1.0f;
    public string url;
    private Material _skyMaterial;
    // Start is called before the first frame update
    void Start()
    {
        _skyMaterial = RenderSettings.skybox;
    }

    public void Switch()
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

        fadeCanvas.QuickFadeOut();

        RenderSettings.skybox = targetMaterial;
        onCompleteAction.Invoke();
    }


}
