using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FlashyPanel : MonoBehaviour
{
    public Image backgroundImage;
    public Color[] colors; 
    public float flashSpeed = 0.5f; 

    private int currentIndex = 0;

    void Start()
    {
        if (backgroundImage == null)
        {
            backgroundImage = GetComponent<Image>();
        }
        InvokeRepeating(nameof(ChangeColor), 0f, flashSpeed);
    }

    //alternate between two colors to create a flashing effect
    void ChangeColor()
    {
        if (colors.Length == 0) return;

        currentIndex = (currentIndex + 1) % colors.Length;
        backgroundImage.color = colors[currentIndex];
    }
}
