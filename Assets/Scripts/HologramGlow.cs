using UnityEngine;

public class HologramGlow : MonoBehaviour
{
    public float colorChangeSpeed = 1f;
    private Material hologramMaterial;

    void Start()
    {
        //get the material from the Renderer
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            hologramMaterial = renderer.material;
            hologramMaterial.EnableKeyword("_EMISSION"); 
        }
    }

    void Update()
    {
        if (hologramMaterial != null)
        {
            //cycle through colors to create color change
            Color newColor = Color.HSVToRGB(Mathf.PingPong(Time.time * colorChangeSpeed, 1), 1, 1);

            //apply color to the emission property
            hologramMaterial.SetColor("_EmissionColor", newColor);
        }
    }
}
