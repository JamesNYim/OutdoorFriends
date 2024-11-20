using UnityEngine;
using Unity.Netcode;
using Unity.Cinemachine;

public class ThirdPersonMovement : NetworkBehaviour {
    public CharacterController controller;
    public float movementSpeed = 5f;
    public float turnSmoothTime = 0.1f;

    private float turnSmoothVelocity;
    [SerializeField] private  CinemachineCamera playerCamera;
    [SerializeField] private AudioListener playerListener;
    public override void OnNetworkSpawn() {
        Debug.Log(IsOwner);
        Debug.Log(gameObject.name + " | " + playerCamera.transform.position);
        if (IsOwner) {
            // Ensure this is the local player's camera
            playerCamera.Priority = 1;
            playerListener.enabled = true; 
        }
        else {
           playerCamera.Priority = 0; 
        }
    }
   
    void Update() {
        if (!IsOwner) return; // Only allow local player movement

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f) {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDirection * movementSpeed * Time.deltaTime);
        }
    }
}

