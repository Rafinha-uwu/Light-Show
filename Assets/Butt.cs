using UnityEngine;

public class ButtonLightController : MonoBehaviour
{
    [Header("Assign the light to control")]
    public Light controlledLight; // The light to toggle

    private bool isPlayerNearby = false;

    void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the trigger zone
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            Debug.Log("Player is near the button. Press 'E' to toggle the light.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the player exited the trigger zone
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            Debug.Log("Player left the button area.");
        }
    }

    void Update()
    {
        // Check if the player is nearby and presses the 'E' key
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (controlledLight != null)
            {
                // Toggle the light's enabled state
                controlledLight.enabled = !controlledLight.enabled;
                Debug.Log($"Light is now {(controlledLight.enabled ? "ON" : "OFF")}.");
            }
            else
            {
                Debug.LogWarning("No light assigned to the script.");
            }
        }
    }
}
