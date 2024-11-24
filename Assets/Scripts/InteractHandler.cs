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
        
    }
    
    void OnTriggerEnter(Collider collision) {
       Debug.Log("Pick Up?: " + collision.gameObject.name);
       pickupBehavior(collision); 
    }
    void OnTriggerExit(Collider  collision) {
        Debug.Log("No longer colliding with: " + collision.gameObject.name);
    }
    void pickupBehavior(Collider collision) {
        if (Input.GetKeyDown(KeyCode.E)) {
            if (collision.gameObject.tag == "Player") {
                Debug.Log("Picking up: " + collision.gameObject.transform.position.x);
            }
        }
        
   }
}



