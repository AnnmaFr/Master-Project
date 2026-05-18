using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialAnchorManager : MonoBehaviour
{
    public List<OVRSpatialAnchor> anchorPrefabs;
    public string sceneIdentifier; // Muss pro Szene eindeutig gesetzt werden!
    public const string NumUuidsPlayerPref = "numUuids";

    private List<OVRSpatialAnchor> anchors = new List<OVRSpatialAnchor>();
    private OVRSpatialAnchor lastCreatedAnchor;
    private AnchorLoader anchorLoader;
    private int nextPrefabIndex = 0;
    private Dictionary<Vector3, OVRSpatialAnchor> existingAnchors = new Dictionary<Vector3, OVRSpatialAnchor>();
    private const float MinDistanceBetweenAnchors = 0.1f;

    private void Awake()
    {
        anchorLoader = GetComponent<AnchorLoader>();
    }

    private void Start()
    {
        LoadSavedAnchors(); // Nur einmal beim Start aufrufen
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            CreateSpatialAnchor();
        }
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            SaveLastCreatedAnchor();
        }
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            UnsaveLastCreatedAnchor();
        }
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
        {
            UnsaveAllAnchors();
        }
        // Entfernt: LoadSavedAnchors() aus Update!
    }

    public void CreateSpatialAnchor()
    {
        if (anchorPrefabs == null || anchorPrefabs.Count == 0)
        {
            Debug.LogError("No anchor prefabs assigned.");
            return;
        }

        var selectedAnchorPrefab = anchorPrefabs[nextPrefabIndex];
        nextPrefabIndex = (nextPrefabIndex + 1) % anchorPrefabs.Count;

        Vector3 position = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
        Quaternion controllerRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
        Vector3 eulerRotation = controllerRotation.eulerAngles;
        Quaternion rotation = Quaternion.Euler(0, eulerRotation.y, 0);

        if (IsPositionOccupied(position))
        {
            Debug.LogWarning("Position already occupied, skipping prefab instantiation.");
            return;
        }

        OVRSpatialAnchor workingAnchor = Instantiate(selectedAnchorPrefab, position, rotation);
        existingAnchors[position] = workingAnchor;

        StartCoroutine(AnchorCreated(workingAnchor));
    }

    private bool IsPositionOccupied(Vector3 position)
    {
        foreach (var existingPosition in existingAnchors.Keys)
        {
            if (Vector3.Distance(existingPosition, position) < MinDistanceBetweenAnchors)
            {
                return true;
            }
        }
        return false;
    }

    private IEnumerator AnchorCreated(OVRSpatialAnchor workingAnchor)
    {
        while (!workingAnchor.Created && !workingAnchor.Localized)
        {
            yield return new WaitForEndOfFrame();
        }

        Guid anchorGuid = workingAnchor.Uuid;
        anchors.Add(workingAnchor);
        lastCreatedAnchor = workingAnchor;
    }

    private void SaveLastCreatedAnchor()
    {
        if (lastCreatedAnchor == null)
            return;

        lastCreatedAnchor.Save((lastCreatedAnchor, success) =>
        {
            if (success)
            {
                SaveUuidToPlayerPrefs(lastCreatedAnchor.Uuid);
            }
        });
    }

    private void SaveUuidToPlayerPrefs(Guid uuid)
    {
        string keyPrefix = $"{sceneIdentifier}_{NumUuidsPlayerPref}";
        if (!PlayerPrefs.HasKey(keyPrefix))
        {
            PlayerPrefs.SetInt(keyPrefix, 0);
        }

        int playerNumUuids = PlayerPrefs.GetInt(keyPrefix);
        PlayerPrefs.SetString($"{sceneIdentifier}_uuid{playerNumUuids}", uuid.ToString());
        PlayerPrefs.SetInt(keyPrefix, ++playerNumUuids);
        PlayerPrefs.Save();
    }

    private void UnsaveLastCreatedAnchor()
    {
        if (lastCreatedAnchor == null)
            return;

        lastCreatedAnchor.Erase((lastCreatedAnchor, success) =>
        {
            if (success)
            {
                existingAnchors.Remove(lastCreatedAnchor.transform.position);
            }
        });
    }

    private void UnsaveAllAnchors()
    {
        foreach (var anchor in anchors)
        {
            anchor.Erase((erasedAnchor, success) =>
            {
                if (success)
                {
                    existingAnchors.Remove(erasedAnchor.transform.position);
                }
            });
        }
        anchors.Clear();
        existingAnchors.Clear();
        ClearAllUuidsFromPlayerPrefs();
    }

    private void ClearAllUuidsFromPlayerPrefs()
    {
        string keyPrefix = $"{sceneIdentifier}_{NumUuidsPlayerPref}";
        if (PlayerPrefs.HasKey(keyPrefix))
        {
            int playerNumUuids = PlayerPrefs.GetInt(keyPrefix);
            for (int i = 0; i < playerNumUuids; i++)
            {
                PlayerPrefs.DeleteKey($"{sceneIdentifier}_uuid{i}");
            }
            PlayerPrefs.DeleteKey(keyPrefix);
            PlayerPrefs.Save();
        }
    }

    public void LoadSavedAnchors()
    {
        anchorLoader.LoadAnchorsByUuid(sceneIdentifier);
    }
}
