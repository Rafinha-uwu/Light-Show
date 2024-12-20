using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSKY : MonoBehaviour
{
    // Reference to the object to rotate
    public GameObject targetObject;

    // Rotation speed
    public float rotationSpeed = 100f;

    // To track if the player is inside the collider
    private bool isPlayerInside = false;

    void Update()
    {
        // Check if the player is inside and pressing Q or E
        if (isPlayerInside && targetObject != null)
        {
            if (Input.GetKey(KeyCode.Q))
            {
                // Rotate left (negative X-axis rotation)
                targetObject.transform.Rotate(Vector3.left * rotationSpeed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.E))
            {
                // Rotate right (positive X-axis rotation)
                targetObject.transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
            }
        }
    }

    // Detect when the player enters the collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
        }
    }

    // Detect when the player exits the collider
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
        }
    }
}
