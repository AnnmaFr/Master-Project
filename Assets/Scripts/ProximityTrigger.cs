using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProximityTrigger : MonoBehaviour
{
    public enum TriggerType { A, B }
    public TriggerType triggerType;

    public AudioSource targetAudio;

    private static int activeTriggers = 0;
    private static AudioSource currentlyPlayingAudio;

    private GameObject overlayImage;
    private GameObject overlayImage2;
    private GameObject overlayImage3;
    public AudioDistortionFilter distortionFilter;
    public Material overlayMaterial;

    private OVRHand leftHand;
    private OVRHand rightHand;
    private Camera mainCamera;

    private Coroutine handTrackingCoroutine;
    private Coroutine overlaySequenceCoroutine;
    public AudioSource audioSource; 
    public AudioClip[] whispers;

    private Coroutine whisperCoroutine;
    private int currentWhisperIndex = 0;

    void Start()
    {
        FindOverlayImages();
        FindHandTracking();
        FindMainCamera();
    }

    //find the overlay images in scene
    void FindOverlayImages()
    {
        GameObject canvas = GameObject.Find("Canvas");
        GameObject canvas2 = GameObject.Find("Canvas2");
        GameObject canvas3 = GameObject.Find("Canvas3");
        if (canvas != null && canvas2 != null)
        {
            overlayImage = canvas2.transform.Find("BlackOverlay")?.gameObject;
            overlayImage2 = canvas.transform.Find("DistortionOverlay")?.gameObject;
            overlayImage3 = canvas3.transform.Find("GrayOverlay")?.gameObject;
        }
        else
        {
            Debug.LogError("Canvas not found! Ensure they exist in the scene.");
        }
    }

    void FindHandTracking()
    {
        GameObject leftHandObj = GameObject.Find("[BuildingBlock] Hand Tracking left");
        GameObject rightHandObj = GameObject.Find("[BuildingBlock] Hand Tracking right");

        if (leftHandObj != null)
            leftHand = leftHandObj.GetComponent<OVRHand>();
        else
            Debug.LogError("Left hand tracking object not found!");

        if (rightHandObj != null)
            rightHand = rightHandObj.GetComponent<OVRHand>();
        else
            Debug.LogError("Right hand tracking object not found!");
    }

    void FindMainCamera()
    {
        GameObject cameraObj = GameObject.Find("CenterEyeAnchor");
        if (cameraObj != null)
            mainCamera = cameraObj.GetComponent<Camera>();
        else
            Debug.LogError("CenterEyeAnchor camera not found! Ensure it's correctly named in the scene.");
    }

    void Update()
    {
        if (leftHand != null && rightHand != null && leftHand.IsTracked && rightHand.IsTracked)
        {
            Vector2 leftHandUV = GetHandScreenUV(leftHand);
            Vector2 rightHandUV = GetHandScreenUV(rightHand);

            Shader.SetGlobalVector("_LeftHandPos", new Vector4(leftHandUV.x, leftHandUV.y, 0, 0));
            Shader.SetGlobalVector("_RightHandPos", new Vector4(rightHandUV.x, rightHandUV.y, 0, 0));
        }
    }

    Vector2 GetHandScreenUV(OVRHand hand)
    {
        if (mainCamera == null)
        {
            Debug.LogError("Main camera is not assigned.");
            return Vector2.zero;
        }

        Vector3 handWorldPos = hand.transform.position;
        Vector3 handViewportPos = mainCamera.WorldToViewportPoint(handWorldPos);
        return new Vector2(handViewportPos.x, handViewportPos.y);
    }

    //create flickering effect by briefly enabling and disabling black overlay over camera. At the same time, enable distortion effect of audio
    IEnumerator OverlaySequence()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3.5f, 5.5f));

            if (overlayImage)
            {
                overlayImage.SetActive(true);
                distortionFilter.distortionLevel = Mathf.Clamp01(0.9f);
                yield return new WaitForSeconds(Random.Range(0.1f, 0.25f));

                overlayImage.SetActive(false);
                distortionFilter.distortionLevel = Mathf.Clamp01(0.0f);
                yield return new WaitForSeconds(Random.Range(2.5f, 3.5f));

                int flickerCount = Random.Range(2, 5);
                for (int i = 0; i < flickerCount; i++)
                {
                    overlayImage.SetActive(true);
                    distortionFilter.distortionLevel = Mathf.Clamp01(0.9f);
                    yield return new WaitForSeconds(Random.Range(0.01f, 0.1f));

                    overlayImage.SetActive(false);
                    distortionFilter.distortionLevel = Mathf.Clamp01(0.0f);
                    yield return new WaitForSeconds(Random.Range(0.01f, 0.1f));
                }
            }
        }
    }

    //display distorted material that looks like noise around the user's hands
    IEnumerator HandTrackingCheck()
    {
        while (true)
        {
            bool handsTracked = (leftHand != null && leftHand.IsTracked) || (rightHand != null && rightHand.IsTracked);

            if (overlayImage2)
                overlayImage2.SetActive(handsTracked);

            yield return new WaitForSeconds(0.2f);
        }
    }

    //play voices at random time intervals
    private IEnumerator PlayWhispersInOrder()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(5f, 10f));

            if (whispers.Length > 0)
            {
                audioSource.clip = whispers[currentWhisperIndex];
                //audioSource.volume = Random.Range(0.3f, 0.6f);
                audioSource.panStereo = Random.Range(-0.5f, 0.5f);
                audioSource.Play();
                currentWhisperIndex = (currentWhisperIndex + 1) % whispers.Length;
            }
        }
    }

    //when collision is detected, check if it was with A or B
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            activeTriggers++;

            if (currentlyPlayingAudio != null && currentlyPlayingAudio != targetAudio)
            {
                currentlyPlayingAudio.Stop();
            }
            if (targetAudio != null)
            {
                targetAudio.Play();
                currentlyPlayingAudio = targetAudio;
            }

            //if collision with B (negative object), play music and start flickering effect and distortion around hands
            if (triggerType == TriggerType.B)
            {
                if (handTrackingCoroutine == null)
                    handTrackingCoroutine = StartCoroutine(HandTrackingCheck());

                if (overlaySequenceCoroutine == null)
                    overlaySequenceCoroutine = StartCoroutine(OverlaySequence());

                if (whisperCoroutine == null) 
                {
                    whisperCoroutine = StartCoroutine(PlayWhispersInOrder());
                }

                overlayImage3.SetActive(true);
            }

            //if collision with A (positive object), only play music
            if (triggerType == TriggerType.A)
            {
                if (whisperCoroutine == null) 
                {
                    whisperCoroutine = StartCoroutine(PlayWhispersInOrder());
                }
            }
        }
    }

    //when the user moves away and collision ends, stop the music and the effects
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            activeTriggers--;

            if (currentlyPlayingAudio == targetAudio)
            {
                targetAudio.Stop();
                currentlyPlayingAudio = null;
            }

            if (triggerType == TriggerType.B)
            {
                if (handTrackingCoroutine != null)
                {
                    StopCoroutine(handTrackingCoroutine);
                    handTrackingCoroutine = null;
                }

                if (overlaySequenceCoroutine != null)
                {
                    StopCoroutine(overlaySequenceCoroutine);
                    overlaySequenceCoroutine = null;
                }

                if (overlayImage2) overlayImage2.SetActive(false);
                if (overlayImage) overlayImage.SetActive(false);
                overlayImage3.SetActive(false);
                if (distortionFilter) distortionFilter.distortionLevel = 0.0f;

                if (whisperCoroutine != null)
                {
                    StopCoroutine(whisperCoroutine);
                    whisperCoroutine = null;
                }
                audioSource.Stop(); 
            }

            if (triggerType == TriggerType.A)
            {
                if (whisperCoroutine != null)
                {
                    StopCoroutine(whisperCoroutine);
                    whisperCoroutine = null;
                }
                audioSource.Stop(); 
            }
        }
    }
}
