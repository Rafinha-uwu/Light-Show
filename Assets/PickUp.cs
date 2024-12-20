using UnityEngine;

public class PickUp : MonoBehaviour
{
    public Transform player; // Reference to the player
    public Transform holdPosition; // Where the object will be held
    public float pickupRange = 2.0f; // Range for picking up objects
    public KeyCode pickupKey = KeyCode.E; // Key to pick up/drop objects

    private GameObject heldObject; // The currently held object

    void Update()
    {
        if (Input.GetKeyDown(pickupKey))
        {
            Debug.Log("E");
            if (heldObject == null)
            {
                AttemptPickup();
                Debug.Log("Ãttempt");
            }
            else
            {
                DropObject();
            }
        }
    }

    void AttemptPickup()
    {
        RaycastHit hit;
        if (Physics.Raycast(player.position, player.forward, out hit, pickupRange))
        {
            Debug.Log("Raycast hit: " + hit.collider.name); // Logs what the raycast hit
            if (hit.collider.CompareTag("Pickup"))
            {
                PickupObject(hit.collider.gameObject);
            }
        }
        else
        {
            Debug.Log("Raycast did not hit anything.");
        }
    }

    void PickupObject(GameObject obj)
    {
        heldObject = obj;
        Rigidbody objRigidbody = heldObject.GetComponent<Rigidbody>();
        if (objRigidbody != null)
        {
            objRigidbody.isKinematic = true; // Disable physics while held
        }
        heldObject.transform.SetParent(holdPosition); // Set as a child of the hold position
        heldObject.transform.localPosition = Vector3.zero; // Align position
        heldObject.transform.localRotation = Quaternion.identity; // Align rotation
    }

    void DropObject()
    {
        if (heldObject != null)
        {
            Rigidbody objRigidbody = heldObject.GetComponent<Rigidbody>();
            if (objRigidbody != null)
            {
                objRigidbody.isKinematic = false; // Re-enable physics
            }
            heldObject.transform.SetParent(null); // Remove parent
            heldObject = null;
        }
    }
}
