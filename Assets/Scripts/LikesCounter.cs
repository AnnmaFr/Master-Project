using UnityEngine;
using TMPro;

public class LikesCounter : MonoBehaviour
{
    public TextMeshProUGUI likesText;  
    public ParticleSystem particleSystem; 
    public int maxLikes = 100;        
    public float burstInterval = 1.0f; 
    public int minBurst = 5;          
    public int maxBurst = 20;         
    public float animationSpeed = 0.05f; 

    private int currentLikes = 0;     
    private float burstTimer = 0f;   
    private float incrementTimer = 0f; 

    void Start()
    {
        ResetCounter(); 
    }

    void Update()
    {
        //increment counter over time, including random increments
        incrementTimer += Time.deltaTime;
        if (incrementTimer >= animationSpeed)
        {
            IncrementCounter(Random.Range(1, 3));
            incrementTimer = 0f;
        }

        //handle random bursts of likes that shuld make likes feel more realistic
        burstTimer += Time.deltaTime;
        if (burstTimer >= burstInterval)
        {
            IncrementCounter(Random.Range(minBurst, maxBurst));
            burstTimer = 0f;
        }

        //reset if likes reach the maximum
        if (currentLikes >= maxLikes)
        {
            ResetCounter();
        }
    }

    private void IncrementCounter(int amount)
    {
        currentLikes += amount;
        likesText.text = currentLikes.ToString();
    }

    private void ResetCounter()
    {
        currentLikes = 0;
        likesText.text = currentLikes.ToString();
    }
}
