using UnityEngine;
using UnityEngine.UI;

public class OverlayController : MonoBehaviour
{
    public Image overlayImage; 
    public Color defaultColor = new Color(0, 0, 0, 0); 
    public Color grayColor = new Color(0.5f, 0.5f, 0.5f, 0.5f); 
    public Color lightColor = new Color(1f, 1f, 0.8f, 0.5f); 

    //set the overlay color
    public void SetOverlayColor(Color color)
    {
        overlayImage.color = color;
    }

    //clear the overlay
    public void ClearOverlay()
    {
        overlayImage.color = defaultColor;
    }
}
