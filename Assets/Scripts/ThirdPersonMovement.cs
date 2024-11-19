using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform camTransform;
    public float movementSpeed = 5f;
    public float turnSmoothTime = 0.1f;

    private float turnSmoothVelocity;
    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical =  Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.5f) {
            float lookAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camTransform.eulerAngles.y; 
            float angle = Mathf.SmoothDampAngle(
                transform.eulerAngles.y,
                lookAngle,
                ref turnSmoothVelocity,
                turnSmoothTime
            );
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = (Quaternion.Euler(0f, lookAngle, 0f) * Vector3.forward).normalized;
            controller.Move(moveDirection * movementSpeed * Time.deltaTime);
        }
    }
}
