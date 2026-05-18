using UnityEngine;

public class AutoDeactivate : MonoBehaviour
{
    public float delay = 5f; // Time in seconds before deactivating

    private void OnEnable()
    {
        CancelInvoke(); // Clear any previous invoke calls
        Invoke(nameof(Deactivate), delay);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
