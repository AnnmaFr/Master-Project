using UnityEngine;
using TMPro; 

public class Keyboard : MonoBehaviour
{
    public GameObject keyboard; 
    public TMP_InputField inputField;

    public void ShowKeyboard()
    {
        keyboard.SetActive(true);
        inputField.ActivateInputField();
    }
}
