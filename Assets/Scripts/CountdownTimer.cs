using System.Collections;
using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject countdownPanel;
    public TMP_Text timerText;
    public GameObject nextPanel;
    public GameObject notification;
    public GameObject mascot;

    [Header("Audio Sources")]
    public AudioSource notificationSoundSource;
    public AudioSource notificationVoiceSource;
    public AudioSource closingSound;

    [Header("Timing")]
    public float countdownTime = 30f;
    public float notificationDelay = 10f;

    [Header("Trigger Object")]
    public GameObject triggerObject;

    [Header("Blocking Panel")]
    public GameObject blockingPanel; // <- Panel, das nicht aktiv sein darf

    private bool hasStarted = false;

    private void Start()
    {
        //initialize UI elements
        notification.SetActive(false);
        nextPanel.SetActive(false);
        mascot.SetActive(false);
        countdownPanel.SetActive(true);
    }

    private void Update()
    {
        //start countdown as soon as instruction panel is closed
        if (!hasStarted && triggerObject != null && !triggerObject.activeSelf)
        {
            hasStarted = true;
            StartCoroutine(StartCountdown());
            StartCoroutine(ShowNotification());
        }

        //disable all objects when time is up
        if (!countdownPanel.activeSelf)
        {
            DisableAllAttentionGrabObjects();
        }
    }

    private IEnumerator StartCountdown()
    {
        float remainingTime = countdownTime;

        while (remainingTime > 0)
        {
            timerText.text = $"{remainingTime:F1}";
            yield return new WaitForSeconds(1f);
            remainingTime--;
        }

        //close data settings panel when time is up and condition is met
        countdownPanel.SetActive(false);

        if (blockingPanel == null || !blockingPanel.activeSelf)
        {
            nextPanel.SetActive(true);
            closingSound.Play();
        }
    }

    private IEnumerator ShowNotification()
    {
        yield return new WaitForSeconds(notificationDelay);

        //show pop-up notification and mascot
        if (countdownPanel.activeSelf)
        {
            notification.SetActive(true);
            mascot.SetActive(true);
        }

        if (notificationSoundSource)
            notificationSoundSource.Play();

        yield return new WaitForSeconds(notificationSoundSource.clip.length);

        if (notificationVoiceSource)
            notificationVoiceSource.Play();
    }

    private void DisableAllAttentionGrabObjects()
    {
        GameObject[] attentionGrabObjects = GameObject.FindGameObjectsWithTag("AttentionGrab");

        foreach (GameObject obj in attentionGrabObjects)
        {
            if (obj.activeSelf)
                obj.SetActive(false);
        }

        Debug.Log("All 'AttentionGrab' objects have been disabled.");
    }
}
