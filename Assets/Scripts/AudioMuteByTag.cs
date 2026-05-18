using UnityEngine;

public class AudioMuteByTag : MonoBehaviour
{
    [Tooltip("Dieses Objekt steuert, ob Audios gemutet oder unmutet sind.")]
    public GameObject watchedObject;

    [Tooltip("Tag der AudioSources, die betroffen sind (z.?B. 'Audio')")]
    public string audioTag = "Audio";

    void Update()
    {
        if (watchedObject == null) return;

        bool shouldMute = watchedObject.activeInHierarchy;

        // Jedes Frame alle passenden AudioSources holen und stumm/laut schalten
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(audioTag);
        foreach (GameObject obj in taggedObjects)
        {
            AudioSource source = obj.GetComponent<AudioSource>();
            if (source != null && source.mute != shouldMute)
            {
                source.mute = shouldMute;
            }
        }
    }
}
