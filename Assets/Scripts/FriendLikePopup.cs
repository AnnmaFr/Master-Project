using System.Collections;
using UnityEngine;
using TMPro;

public class FriendLikePopup : MonoBehaviour
{
    public TextMeshProUGUI likeText;
    public AudioSource likeSound; 
    public float duration = 2f; 
    public float fadeSpeed = 2f;

    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        gameObject.SetActive(false); 
    }

    //display name of friend
    public void ShowPopup(string friendName)
    {
        likeText.text = friendName;
        gameObject.SetActive(true);
        likeSound.Play();
        StartCoroutine(FadeInAndOut());
    }

    //fade the name in and out
    IEnumerator FadeInAndOut()
    {
        canvasGroup.alpha = 1;
        yield return new WaitForSeconds(duration);

        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
