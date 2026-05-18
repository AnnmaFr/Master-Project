using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class SceneSwitch : MonoBehaviour
{
    public Toggle scene1Toggle;
    public string sceneName;
    private bool isInitialized = false;

    void Start()
    {
        isInitialized = true;

        if (scene1Toggle != null)
        {
            scene1Toggle.onValueChanged.AddListener(OnToggleValueChanged);
        }
    }

    void OnToggleValueChanged(bool isOn)
    {
        if (isOn && isInitialized)
        {
            LoadScene(sceneName);
        }
    }

    //load specific scene
    void LoadScene(string sceneName)
    {
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
