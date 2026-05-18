using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpinWheel : MonoBehaviour
{
    [Header("Spin Settings")]
    public float spinDuration = 2f; 
    public Toggle spinToggle; 

    [Header("Effects")]
    public ParticleSystem confettiEffect; 
    public AudioSource spinSound;
    public AudioSource partyHorn;
    public AudioSource yay;
    public AudioSource warningSound; 
    public AudioSource secondWarningSound; 

    [Header("UI Elements")]
    public GameObject overlayPanel; 
    public TMP_Text overlayText; 
    public Button closeButton; 
    public GameObject notificationPanel;
    public GameObject mascot;


    private bool isSpinning = false;
    private bool hasSpun = false;
    private float[] stopAngles = { 45f, 135f, 225f, 315f }; 
    private int[] prizeAmounts = { 10, 50, 20, 100 }; 
    private float targetAngle;

    void Start()
    {
        if (spinToggle != null)
            spinToggle.onValueChanged.AddListener(delegate { ToggleSpin(); });

        if (closeButton != null)
            closeButton.onClick.AddListener(HideOverlay);

        overlayPanel.SetActive(false);
        notificationPanel.SetActive(true);

        //start the routine to check every 5 seconds
        StartCoroutine(CheckToggleRoutine());

        mascot.transform.position = new Vector3(
                    notificationPanel.transform.position.x,
                    mascot.transform.position.y,
                    notificationPanel.transform.position.z
                );
    }

    void ToggleSpin()
    {
        if (spinToggle.isOn && !isSpinning)
        {
            //if user has spun wheel
            hasSpun = true;
            StartCoroutine(Spin());
            mascot.SetActive(false);
        }
    }

    //sound is constantly played every 3 seconds if wheel is not spun
    IEnumerator CheckToggleRoutine()
    {
        bool playFirstWarning = true; // Toggle between sounds

        while (true)
        {
            yield return new WaitForSeconds(3f);

            if (!spinToggle.isOn && !isSpinning && !hasSpun)
            {
                if (playFirstWarning)
                {
                    if (warningSound != null)
                        warningSound.Play();
                }
                else
                {
                    if (secondWarningSound != null)
                        secondWarningSound.Play();
                }
                playFirstWarning = !playFirstWarning;
            }
        }
    }

    //spins the wheel and stops at random price
    //price is displayed and sound effects are played and confetti effect appears when wheel stops spinning
    IEnumerator Spin()
    {
        isSpinning = true;
        spinToggle.interactable = false;

        //play spin sound
        if (spinSound != null)
        {
            spinSound.pitch = 1.5f;
            spinSound.Play();
        }

        float startAngle = transform.localEulerAngles.z;
        int randomIndex = Random.Range(0, stopAngles.Length);
        float desiredAngle = stopAngles[randomIndex];

        float diff = (startAngle - desiredAngle + 360f) % 360f;
        if (Mathf.Approximately(diff, 0f))
            diff = 360f;

        int extraRotations = Random.Range(3, 6);
        float totalDegrees = extraRotations * 360f + diff;
        targetAngle = startAngle - totalDegrees;

        float elapsedTime = 0f;
        while (elapsedTime < spinDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / spinDuration;
            t = 1 - (1 - t) * (1 - t);
            float currentAngle = Mathf.Lerp(startAngle, targetAngle, t);

            //spin sound slows down
            if (spinSound != null)
                spinSound.pitch = Mathf.Lerp(1.5f, 0.5f, t);

            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, currentAngle);
            yield return null;
        }

        float finalAngle = ((targetAngle % 360f) + 360f) % 360f;
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, finalAngle);
        isSpinning = false;

        if (spinSound != null)
        {
            spinSound.pitch = 1.0f;
            spinSound.Stop();
        }

        if (confettiEffect != null)
            confettiEffect.Play();

        if (partyHorn != null)
            partyHorn.Play();

        if (yay != null)
            yay.Play();

        spinToggle.isOn = false;
        spinToggle.interactable = true;

        ShowOverlay(prizeAmounts[randomIndex]);
    }



    //shows the overlay with the prize amount and starts countdown to close notification panel after 5 seconds
    void ShowOverlay(int prizeAmount)
    {
        if (overlayPanel != null && overlayText != null)
        {
            overlayText.text = $"Herzlichen Glückwunsch! Du hast {prizeAmount}€ gewonnen!";
            overlayPanel.SetActive(true);
            StartCoroutine(AutoCloseNotificationPanel());
        }
        else
        {
            Debug.LogError("Overlay panel or text not assigned.");
        }
    }

    void HideOverlay()
    {
        overlayPanel.SetActive(false);
        notificationPanel.SetActive(false); 
    }

    //closes notification panel and hides mascot automatically after 5 seconds
    IEnumerator AutoCloseNotificationPanel()
    {
        yield return new WaitForSeconds(5f);

        if (notificationPanel != null)
        {
            notificationPanel.SetActive(false);
            //mascot.SetActive(false);
        }
        else
        {
            Debug.LogError("Notification panel not assigned.");
        }
    }
}
