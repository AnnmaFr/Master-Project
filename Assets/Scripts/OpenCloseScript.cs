using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OpenCloseScript : MonoBehaviour
{
    public Toggle toggle;
    private bool isInitialized = false;
    public GameObject open1;
    public GameObject open2;
    public GameObject close1;
    public GameObject close2;

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
        //open object 1 and 2, close object 3 and 4
        if (isOn && isInitialized)
        {
            open1.SetActive(true);
            open2.SetActive(true);
            close1.SetActive(false);
            close2.SetActive(false);
        }
    }
}
