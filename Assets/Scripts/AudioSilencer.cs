using UnityEngine;
using System.Collections.Generic;

public class AudioSilencer : MonoBehaviour
{
    [Tooltip("Das Objekt, dessen Aktivierung den Ton anderer Objekte stummschalten soll")]
    public GameObject watchedObject;

    [Tooltip("Optional: z.?B. ein Maskottchen, das beim Video ausgeblendet wird")]
    public GameObject mascot;

    [Tooltip("Das Objekt, das aktiv sein muss, damit das Maskottchen zurückkommt")]
    public GameObject conditionalObject;

    [Tooltip("AudioSources, die stumm geschaltet werden sollen, wenn das watchedObject aktiv ist")]
    public List<AudioSource> targetAudioSources;

    private bool previousActiveState;

    void Start()
    {
        if (watchedObject == null)
        {
            Debug.LogError("Kein watchedObject gesetzt.");
            enabled = false;
            return;
        }

        previousActiveState = watchedObject.activeInHierarchy;
        UpdateAudioState(previousActiveState);
    }

    void Update()
    {
        bool currentActiveState = watchedObject.activeInHierarchy;
        if (currentActiveState != previousActiveState)
        {
            UpdateAudioState(currentActiveState);
            previousActiveState = currentActiveState;
        }
    }

    private void UpdateAudioState(bool isWatchedActive)
    {
        foreach (AudioSource src in targetAudioSources)
        {
            if (src == null) continue;
            src.mute = isWatchedActive;
        }

        if (mascot != null)
        {
            if (isWatchedActive)
            {
                mascot.SetActive(false);
            }
            else
            {
                // Nur wieder aktivieren, wenn das optionale Objekt aktiv ist
                bool condition = conditionalObject != null && conditionalObject.activeInHierarchy;
                mascot.SetActive(condition);
            }
        }
    }
}