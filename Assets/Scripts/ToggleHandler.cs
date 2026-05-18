using UnityEngine;
using UnityEngine.UI;

public class ToggleHandler : MonoBehaviour
{
    public Toggle toggle;
    public GameObject objectOn;
    public GameObject objectOff;

    void Start()
    {
        OnToggleValueChanged(toggle.isOn);
        toggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    void OnToggleValueChanged(bool isOn)
    {
        objectOn.SetActive(isOn);
        objectOff.SetActive(!isOn);
    }

    void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(OnToggleValueChanged);
    }
}
