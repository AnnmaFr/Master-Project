using UnityEngine;
using System.Collections;

public class TriggerAudio : MonoBehaviour
{
    public AudioSource audioSource1;
    public AudioSource audioSource2;
    public float cooldownTime = 0.5f;

    private static AudioSource currentlyPlayingAudio1;
    private static AudioSource currentlyPlayingAudio2;
    private static TriggerAudio latestTrigger;
    private static bool isOnCooldown = false;

    //if collision detected, play sound
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isOnCooldown)
        {
            StartCoroutine(TriggerAudioWithCooldown());
        }
    }

    private IEnumerator TriggerAudioWithCooldown()
    {
        isOnCooldown = true;
        latestTrigger = this;

        //stop previous audios if they exist
        if (currentlyPlayingAudio1 != null && currentlyPlayingAudio1 != audioSource1)
        {
            currentlyPlayingAudio1.Stop();
        }

        if (currentlyPlayingAudio2 != null && currentlyPlayingAudio2 != audioSource2)
        {
            currentlyPlayingAudio2.Stop();
        }

        //play first audio if available and not playing
        if (audioSource1 != null && !audioSource1.isPlaying)
        {
            audioSource1.Play();
            currentlyPlayingAudio1 = audioSource1;
        }

        //play second audio if available and not playing
        if (audioSource2 != null && !audioSource2.isPlaying)
        {
            audioSource2.Play();
            currentlyPlayingAudio2 = audioSource2;
        }

        yield return new WaitForSeconds(cooldownTime);
        isOnCooldown = false;
    }

    //if collision ends, stop sound
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(HandleTriggerExit());
        }
    }

    private IEnumerator HandleTriggerExit()
    {
        yield return new WaitForSeconds(0.1f);

        if (latestTrigger == this)
        {
            if (currentlyPlayingAudio1 != null)
            {
                currentlyPlayingAudio1.Stop();
                currentlyPlayingAudio1 = null;
            }

            if (currentlyPlayingAudio2 != null)
            {
                currentlyPlayingAudio2.Stop();
                currentlyPlayingAudio2 = null;
            }
        }
    }
}
