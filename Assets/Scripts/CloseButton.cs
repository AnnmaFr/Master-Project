using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CloseButton : MonoBehaviour
{
    public Toggle toggle;
    private bool isInitialized = false;
    public GameObject panel;

    void Start()
    {
        isInitialized = true;
        if (toggle != null)
        {
            toggle.onValueChanged.AddListener(OnToggleValueChanged);
        }
    }

    private void OnToggleValueChanged(bool isOn)
    {
        if (isOn && isInitialized)
        {
            panel.SetActive(false);
        }
    }
}
