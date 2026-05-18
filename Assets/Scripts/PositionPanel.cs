using UnityEngine;

public class PositionPanel : MonoBehaviour
{
    public Transform centerEyeAnchor;
    public Vector3 positionOffset = new Vector3(0, 0, 0.5f);
    private float repositionThreshold = 1.0f;
    public float rotationThreshold = 45f;

    private Vector3 lastValidPosition;
    private Quaternion lastValidRotation;
    private bool isPositioned = false;

    void OnEnable()
    {
        isPositioned = false;
        RepositionPanel();
    }

    void Update()
    {
        if (centerEyeAnchor == null)
            return;

        if (!isPositioned || NeedsRepositioning())
        {
            RepositionPanel();
        }
        else
        {
            UpdateRotationIfNeeded();
        }
    }

    private void RepositionPanel()
    {
        Vector3 newPosition = CalculateIdealPosition();
        transform.position = newPosition;
        lastValidPosition = newPosition;

        UpdateRotation();
        isPositioned = true;
    }

    private bool NeedsRepositioning()
    {
        Vector3 idealPosition = CalculateIdealPosition();
        return Vector3.Distance(idealPosition, lastValidPosition) > repositionThreshold;
    }

    private void UpdateRotationIfNeeded()
    {
        Quaternion idealRotation = CalculateIdealRotation();
        float angleDifference = Quaternion.Angle(idealRotation, lastValidRotation);

        if (angleDifference > rotationThreshold)
        {
            UpdateRotation();
        }
    }

    private void UpdateRotation()
    {
        Quaternion newRotation = CalculateIdealRotation();
        transform.rotation = newRotation;
        lastValidRotation = newRotation;
    }

    private Quaternion CalculateIdealRotation()
    {
        Vector3 directionToUser = centerEyeAnchor.position - transform.position;
        directionToUser.y = 0;
        return Quaternion.LookRotation(-directionToUser);
    }

    private Vector3 CalculateIdealPosition()
    {
        return centerEyeAnchor.position +
               centerEyeAnchor.forward * positionOffset.z +
               centerEyeAnchor.right * positionOffset.x +
               centerEyeAnchor.up * positionOffset.y;
    }
}