using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class SceneSwitch1 : MonoBehaviour
{
    public Toggle scene1Toggle;
    private bool isInitialized = false;
    public GameObject cube;

    void Start()
    {
        isInitialized = true;
        if (scene1Toggle != null)
            scene1Toggle.onValueChanged.AddListener((isOn) => { if (isOn && isInitialized) cube.SetActive(true); });
    }
}
