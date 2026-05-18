using UnityEngine;
using UnityEngine.SceneManagement;

public class FollowHead : MonoBehaviour
{
    private GameObject panelPrefab; 
    public float distanceFromUser = 2.0f; 
    public float heightOffset = 1.0f; 
    private Transform centerEyeAnchor; 

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        InitializeCameraRig();
        InstantiateAndPositionPanel();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene loaded: {scene.name}");
        InitializeCameraRig();
        InstantiateAndPositionPanel();
    }

    private void InitializeCameraRig()
    {
        GameObject ovrCameraRig = GameObject.Find("OVRCameraRig");
        if (ovrCameraRig == null)
        {
            Debug.LogError("OVRCameraRig not found in the scene.");
            return;
        }

        centerEyeAnchor = ovrCameraRig.transform.Find("TrackingSpace/CenterEyeAnchor");
        if (centerEyeAnchor == null)
        {
            Debug.LogError("CenterEyeAnchor not found in OVRCameraRig.");
        }
    }

    private void InstantiateAndPositionPanel()
    {
        if (panelPrefab == null)
        {
            Debug.LogError("Panel prefab not assigned.");
            return;
        }

        if (centerEyeAnchor == null)
        {
            Debug.LogError("CenterEyeAnchor is not set.");
            return;
        }

        GameObject panel = Instantiate(panelPrefab);
        panel.SetActive(true);
        Vector3 newPosition = centerEyeAnchor.position + centerEyeAnchor.forward * distanceFromUser;
        newPosition.y = centerEyeAnchor.position.y + heightOffset;
        panel.transform.position = newPosition;
        panel.transform.rotation = Quaternion.LookRotation(centerEyeAnchor.forward, Vector3.up);

        Debug.Log("Panel instantiated and positioned.");
    }
}
