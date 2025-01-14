using UnityEngine;

public class LightControl : MonoBehaviour
{
    public Light targetLight; // The light to be controlled
    public float intensityStep = 0.5f; // How much the intensity increases per second
    public float maxIntensity = 3.0f; // Maximum light intensity
    private float initialIntensity; // Initial light intensity

    public Color[] colors = { Color.white, Color.red, Color.green, Color.blue, Color.yellow };
    private int colorIndex = 0; // Current color index
    private Color initialColor; // Initial light color
    private bool playerInside = false; // To check if the player is inside the trigger

    void Start()
    {
        if (targetLight != null)
        {
            initialIntensity = targetLight.intensity;
            initialColor = targetLight.color;
        }
    }

    void Update()
    {
        if (playerInside && targetLight != null)
        {
            // Hold E to increase intensity
            if (Input.GetKey(KeyCode.E))
            {
                targetLight.intensity += intensityStep * Time.deltaTime;

                if (targetLight.intensity > maxIntensity)
                {
                    targetLight.intensity = initialIntensity; // Reset to initial intensity
                }
            }

            // Press Q to change color
            if (Input.GetKeyDown(KeyCode.Q))
            {
                colorIndex++;
                if (colorIndex >= colors.Length)
                {
                    colorIndex = 0; // Reset to initial color
                    targetLight.color = initialColor;
                }
                else
                {
                    targetLight.color = colors[colorIndex];
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the object entering the trigger is the player
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the player leaves the trigger
        {
            playerInside = false;
        }
    }
}
