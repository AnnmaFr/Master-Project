using System.Collections;
using UnityEngine;

public class OverlayEffect : MonoBehaviour
{
    private GameObject overlayImage;
    private GameObject overlayImage2;
    public AudioDistortionFilter distortionFilter;
    public Material overlayMaterial;

    private OVRHand leftHand;
    private OVRHand rightHand;
    private Camera mainCamera;

    void Start()
    {
        FindOverlayImages();
        FindHandTracking();
        FindMainCamera();

        StartCoroutine(OverlaySequence());
        StartCoroutine(HandTrackingCheck());
    }

    void FindOverlayImages()
    {
        GameObject canvas = GameObject.Find("Canvas");
        GameObject canvas2 = GameObject.Find("Canvas2");
        if (canvas != null && canvas2 != null)
        {
            overlayImage = canvas2.transform.Find("BlackOverlay")?.gameObject;
            overlayImage2 = canvas.transform.Find("DistortionOverlay")?.gameObject;
        }
        else
        {
            Debug.LogError("Canvas or Canvas2 not found! Ensure they exist in the scene.");
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
}
