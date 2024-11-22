using UnityEngine;

public class InteractHandler : MonoBehaviour
{
    [SerializeField] private LayerMask interactLayer;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform handTransform;
    [SerializeField] private float interactRange;
    private Rigidbody itemRigidbody;
    private Collider itemCollider;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) {
            Ray interactRay = new Ray(playerTransform.position, playerTransform.forward);
            if (Physics.Raycast(interactRay, out RaycastHit interactInfo, interactRange, interactLayer)) {
                pickupBehavior(interactInfo);
                Debug.Log(interactInfo.distance);
            }
        }
        // Holding the item in our hand
        if (itemRigidbody) {
            itemRigidbody.position = handTransform.position;
            itemRigidbody.rotation = handTransform.rotation;
        }
    }

    void pickupBehavior(RaycastHit interactInfo) {
        if (itemRigidbody) {
            // This means that we are holding something already.
            itemRigidbody.isKinematic = false;
            itemCollider.enabled = true;

            itemRigidbody = interactInfo.rigidbody;
            itemCollider = interactInfo.collider;

            itemRigidbody.isKinematic = true;
            itemCollider.enabled = false;
        }
        else {
            itemRigidbody = interactInfo.rigidbody;
            itemCollider = interactInfo.collider;

            itemRigidbody.isKinematic = true;
            itemCollider.enabled = false;
        }
        
    }
}
