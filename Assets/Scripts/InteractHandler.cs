using UnityEngine;

public class InteractHandler : MonoBehaviour
{
    [SerializeField] private int interactLayer;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform handTransform;
    [SerializeField] private float interactRange;
    private Collider itemCollider;
    private bool canInteract;

    void Start() {
        canInteract = false;
    }
    void Update()
    {
       if (Input.GetKeyDown(KeyCode.E) && canInteract) {
           pickupBehavior();
       } 
       else {
        
       }
    }
    
    void OnTriggerEnter(Collider collision) {
       Debug.Log("Pick Up?: " + collision.gameObject.name);
       itemCollider = collision;
       if (itemCollider.gameObject.layer == interactLayer) {
            canInteract = true;
       }
       else {
        canInteract = false;

       }
    }
    void OnTriggerExit(Collider  collision) {
        canInteract = false;
    }
    void pickupBehavior() {
         itemCollider.gameObject.transform.position = handTransform.position;
         itemCollider.gameObject.transform.rotation = handTransform.rotation;
         itemCollider.gameObject.transform.SetParent(handTransform, true);
   }
}



