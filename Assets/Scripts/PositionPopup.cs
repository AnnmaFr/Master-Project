using UnityEngine;

public class PositionPopup : MonoBehaviour
{
    public Transform centerEyeAnchor; 
    public Vector3 positionOffset = new Vector3(0, 0, 2.0f);

    private bool isActive = false;

    void OnEnable()
    {
        isActive = true;
        RecalculatePosition();
    }

    void OnDisable()
    {
        isActive = false;
    }

    private void RecalculatePosition()
    {
        if (isActive && centerEyeAnchor != null)
        {
            //calculate target position based on the user's current head position
            Vector3 targetPosition = centerEyeAnchor.position +
                                     centerEyeAnchor.forward * positionOffset.z + 
                                     centerEyeAnchor.right * positionOffset.x +  
                                     centerEyeAnchor.up * positionOffset.y;  

            transform.position = targetPosition;
            transform.rotation = Quaternion.LookRotation(transform.position - centerEyeAnchor.position);
        }
    }
}
