using System;
using System.Collections.Generic;
using UnityEngine;

public class AnchorLoader : MonoBehaviour
{
    private SpatialAnchorManager spatialAnchorManager;
    private Action<OVRSpatialAnchor.UnboundAnchor, bool> _onLoadAnchor;
    private Dictionary<Guid, int> uuidToPrefabIndex;
    private Dictionary<Vector3, OVRSpatialAnchor> existingAnchors;
    private const float MinDistanceBetweenAnchors = 0.1f;

    private void Awake()
    {
        spatialAnchorManager = GetComponent<SpatialAnchorManager>();
        _onLoadAnchor = OnLocalized;
        uuidToPrefabIndex = new Dictionary<Guid, int>();
        existingAnchors = new Dictionary<Vector3, OVRSpatialAnchor>();
    }

    public void LoadAnchorsByUuid(string sceneIdentifier)
    {
        string keyPrefix = $"{sceneIdentifier}_{SpatialAnchorManager.NumUuidsPlayerPref}";
        if (!PlayerPrefs.HasKey(keyPrefix))
        {
            PlayerPrefs.SetInt(keyPrefix, 0);
        }

        var playerUuidCount = PlayerPrefs.GetInt(keyPrefix);
        if (playerUuidCount == 0) return;

        var uuids = new Guid[playerUuidCount];
        for (int i = 0; i < playerUuidCount; ++i)
        {
            var uuidKey = $"{sceneIdentifier}_uuid{i}";
            var currentUuid = PlayerPrefs.GetString(uuidKey);
            uuids[i] = new Guid(currentUuid);

            uuidToPrefabIndex[uuids[i]] = i % spatialAnchorManager.anchorPrefabs.Count;
        }

        Load(new OVRSpatialAnchor.LoadOptions
        {
            Timeout = 0,
            StorageLocation = OVRSpace.StorageLocation.Local,
            Uuids = uuids
        });
    }

    private void Load(OVRSpatialAnchor.LoadOptions options)
    {
        OVRSpatialAnchor.LoadUnboundAnchors(options, anchors =>
        {
            if (anchors == null) return;

            foreach (var anchor in anchors)
            {
                if (anchor.Localized)
                {
                    _onLoadAnchor(anchor, true);
                }
                else if (!anchor.Localizing)
                {
                    anchor.Localize(_onLoadAnchor);
                }
            }
        });
    }

    private void OnLocalized(OVRSpatialAnchor.UnboundAnchor unboundAnchor, bool success)
    {
        if (!success) return;

        var pose = unboundAnchor.Pose;

        if (IsPositionOccupied(pose.position))
        {
            Debug.LogWarning("Position already occupied, skipping prefab instantiation.");
            return;
        }

        if (!uuidToPrefabIndex.TryGetValue(unboundAnchor.Uuid, out var prefabIndex))
        {
            Debug.LogError("UUID not mapped to a prefab index.");
            return;
        }

        var selectedAnchorPrefab = spatialAnchorManager.anchorPrefabs[prefabIndex];
        var spatialAnchor = Instantiate(selectedAnchorPrefab, pose.position, pose.rotation);
        existingAnchors[pose.position] = spatialAnchor;

        unboundAnchor.BindTo(spatialAnchor);
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
}
