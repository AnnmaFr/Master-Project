using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NotificationSpawner : MonoBehaviour
{
    public GameObject[] notifications; 
    public Transform playerHead; 
    public AudioClip[] notificationSounds; 
    public AudioSource[] audioSources; 
    public Toggle notificationToggle; 

    private Coroutine notificationRoutine;

    void Start()
    {
        if (notificationToggle != null)
        {
            notificationToggle.onValueChanged.AddListener(OnToggleValueChanged);
        }

        //disable all notification in the beginning
        foreach (var notification in notifications)
        {
            if (notification != null)
                notification.SetActive(false);
        }
    }

    public void OnToggleValueChanged(bool isOn)
    {
        if (isOn)
        {
            //if save button was pressed, display notifications to distract user
            if (notificationRoutine == null)
            {
                notificationRoutine = StartCoroutine(ShowNotifications());
            }
        }
        else
        {
            if (notificationRoutine != null)
            {
                StopCoroutine(notificationRoutine);
                notificationRoutine = null;
            }

            foreach (var notification in notifications)
            {
                if (notification != null)
                    notification.SetActive(false);
            }
        }
    }

    private IEnumerator ShowNotifications()
    {
        //display all notification but wait for 5 seconds between each notification
        for (int i = 0; i < notifications.Length; i++)
        {
            if (!notificationToggle.isOn)
                yield break;

            if (notifications[i] != null)
                notifications[i].SetActive(true);

            //play notification sound
            if (i < audioSources.Length && i < notificationSounds.Length)
            {
                audioSources[i].PlayOneShot(notificationSounds[i]);
            }

            yield return new WaitForSeconds(5f);
        }

        notificationRoutine = null;
    }
}
