using UnityEngine;
using UnityEngine.UI;

public class OpenURL : MonoBehaviour
{
    public Toggle toggle;
    private bool isInitialized = false;
    public string url = "https://example.com";

    void Start()
    {
        isInitialized = true;
        if (toggle != null)
            toggle.onValueChanged.AddListener((isOn) => { if (isOn && isInitialized) Application.OpenURL(url); });
    }
}
