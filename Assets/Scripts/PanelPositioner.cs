using UnityEngine;
using System.Collections;

public class PanelPositioner : MonoBehaviour
{
    public OVRCameraRig cameraRig;
    public float distanceFromUser = 1.5f;
    public float yOffset = -0.3f;

    private void OnEnable()
    {
        StartCoroutine(PositionPanelCoroutine());
    }

    private IEnumerator PositionPanelCoroutine()
    {
        yield return new WaitForEndOfFrame();

        //keep trying to position panel until successful
        while (true)
        {
            if (cameraRig != null && cameraRig.centerEyeAnchor != null)
            {
                PositionPanel();
                break; 
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    //position panel at eye level
    private void PositionPanel()
    {
        Transform centerEyeAnchor = cameraRig.centerEyeAnchor;
        Vector3 eyePosition = centerEyeAnchor.position;
        Vector3 eyeForward = centerEyeAnchor.forward;

        Vector3 targetPosition = eyePosition + eyeForward * distanceFromUser;
        targetPosition.y = eyePosition.y + yOffset;

        transform.position = targetPosition;
        transform.LookAt(2 * transform.position - eyePosition);

        Debug.Log($"Eye Position: {eyePosition}, Panel Position: {transform.position}");
    }
}
