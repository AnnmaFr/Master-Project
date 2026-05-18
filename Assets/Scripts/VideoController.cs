using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    public Toggle playToggle;
    public Toggle forwardToggle;
    public Toggle rewindToggle;

    public GameObject playIcon;
    public GameObject pauseIcon;
    public GameObject replayIcon;

    public GameObject skipButton;
    public GameObject mainMenuButton;
    public GameObject forwardButton;


    void Start()
    {
        videoPlayer.Play();

        videoPlayer.started += OnVideoStarted;

        playToggle.onValueChanged.RemoveAllListeners();
        forwardToggle.onValueChanged.RemoveAllListeners();
        rewindToggle.onValueChanged.RemoveAllListeners();

        playToggle.isOn = true;

        playToggle.onValueChanged.AddListener(OnPlayToggleChanged);
        forwardToggle.onValueChanged.AddListener(OnForwardToggleChanged);
        rewindToggle.onValueChanged.AddListener(OnRewindToggleChanged);

        videoPlayer.loopPointReached += OnVideoFinished;

        skipButton.SetActive(true);
        mainMenuButton.SetActive(false);
        forwardButton.SetActive(true);
    }

    private void OnVideoStarted(VideoPlayer vp)
    {
        UpdateIcons();
        videoPlayer.started -= OnVideoStarted; 
    }

    private void OnPlayToggleChanged(bool isOn)
    {
        if (isOn)
        {
            bool wasAtEnd = videoPlayer.frame >= (long)videoPlayer.frameCount - 1;

            if (wasAtEnd)
            {
                videoPlayer.time = 0;
                forwardButton.SetActive(true);
                skipButton.SetActive(true);
                mainMenuButton.SetActive(false);
            }

            videoPlayer.Play();
        }
        else
        {
            videoPlayer.Pause();
        }

        UpdateIcons();
    }


    private void OnForwardToggleChanged(bool isOn)
    {
        if (isOn)
        {
            double newTime = videoPlayer.time + 10.0;
            videoPlayer.time = Mathf.Min((float)newTime, (float)(videoPlayer.length - 0.1));
            forwardToggle.isOn = false;

            if (!videoPlayer.isPlaying)
            {
                videoPlayer.Play();
                playToggle.isOn = true;
            }

            UpdateIcons();
        }
    }

    private void OnRewindToggleChanged(bool isOn)
    {
        if (isOn)
        {
            double newTime = videoPlayer.time - 10.0;
            videoPlayer.time = Mathf.Max((float)newTime, 0f);
            rewindToggle.isOn = false;

            if (!videoPlayer.isPlaying)
            {
                videoPlayer.Play();
                playToggle.isOn = true;
            }

            UpdateIcons();
        }
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        playToggle.isOn = false;
        UpdateIcons();

        skipButton.SetActive(false);
        mainMenuButton.SetActive(true);
        forwardButton.SetActive(false);
    }


    private void UpdateIcons()
    {
        bool isPlaying = videoPlayer.isPlaying;
        bool atEnd = (videoPlayer.frame >= (long)videoPlayer.frameCount - 1);

        playIcon.SetActive(!isPlaying && !atEnd);
        pauseIcon.SetActive(isPlaying);
        replayIcon.SetActive(!isPlaying && atEnd);
    }
}
