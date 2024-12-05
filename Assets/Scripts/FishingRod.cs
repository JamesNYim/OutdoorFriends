using UnityEngine;

public class FishingRod : MonoBehaviour
{
    [Header("Casting Settings")]
    public GameObject bobberPrefab;      // Bobber prefab to instantiate
    public Transform castPoint;          // Point from which the bobber is cast
    public float maxCastDistance = 20f;  // Maximum cast distance
    public float chargeRate = 10f;       // Rate at which the cast power charges
    public float reelSpeed = 5f;         // Speed of reeling the bobber back

    private float castPower = 0f;        // Current casting power
    private bool isCharging = false;     // Whether casting power is charging
    private GameObject currentBobber;    // Active bobber in the scene

    void Update()
    {
        HandleCastingInput();
    }

    private void HandleCastingInput()
    {
        // Start charging when the left mouse button is pressed
        if (Input.GetMouseButtonDown(0))
        {
            isCharging = true;
            castPower = 0f;
        }

        // Charge the cast while holding the button
        if (Input.GetMouseButton(0) && isCharging)
        {
            castPower += chargeRate * Time.deltaTime;
            castPower = Mathf.Clamp(castPower, 0f, maxCastDistance);
        }

        // Cast the bobber when the button is released
        if (Input.GetMouseButtonUp(0) && isCharging)
        {
            isCharging = false;
            CastBobber();
        }

        // Reel in the bobber if it exists and the right mouse button is pressed
        if (Input.GetMouseButtonDown(1) && currentBobber != null)
        {
            ReelInBobber();
        }
    }

    private void CastBobber()
    {
        if (bobberPrefab != null && castPoint != null)
        {
            currentBobber = Instantiate(bobberPrefab, castPoint.position, Quaternion.identity);

            Vector3 castDirection = castPoint.forward;  // Adjust based on rod orientation
            Rigidbody rb = currentBobber.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddForce(castDirection * castPower, ForceMode.Impulse);
            }
            else
            {
                // Fallback for non-physics bobber
                currentBobber.transform.position = castPoint.position + castDirection * castPower;
            }

            castPower = 0f;  // Reset cast power
        }
    }

    private void ReelInBobber()
    {
        if (currentBobber != null)
        {
            StartCoroutine(ReelInCoroutine());
        }
    }

    private System.Collections.IEnumerator ReelInCoroutine()
    {
        while (currentBobber != null && Vector3.Distance(currentBobber.transform.position, castPoint.position) > 0.5f)
        {
            currentBobber.transform.position = Vector3.MoveTowards(currentBobber.transform.position, castPoint.position, reelSpeed * Time.deltaTime);
            yield return null;
        }

        if (currentBobber != null)
        {
            Destroy(currentBobber);  // Destroy the bobber when it reaches the rod
        }
    }
}

