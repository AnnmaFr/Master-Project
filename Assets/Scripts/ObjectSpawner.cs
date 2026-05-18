using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject prefabToInstantiate;
    private OVRCameraRig cameraRig; 

    void Start()
    {
        cameraRig = FindObjectOfType<OVRCameraRig>(); 

        if (cameraRig != null && prefabToInstantiate != null)
        {
            InstantiatePrefab();
        }
        else
        {
            Debug.LogError("OVRCameraRig or Prefab is not assigned!");
        }
    }

    void InstantiatePrefab()
    {
        Vector3 eyePosition = cameraRig.centerEyeAnchor.position;
        Vector3 forwardDirection = cameraRig.centerEyeAnchor.forward;  
        Vector3 spawnPosition = eyePosition + forwardDirection * 2.0f; 
        spawnPosition.y = eyePosition.y; 
        Instantiate(prefabToInstantiate, spawnPosition, Quaternion.identity);
    }
}
