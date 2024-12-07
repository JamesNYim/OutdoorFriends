using UnityEngine;

public class FishingRod : MonoBehaviour
{
    public enum FishingRodState { Idle, Charging, Casting, Reeling }

    [Header("Casting Settings")]
    public GameObject bobberPrefab;         // Bobber prefab to instantiate
    public Transform castPoint;             // Starting point of the cast
    public GameObject castMarkerPrefab;     // Prefab for the cast marker
    public LayerMask groundLayer;           // LayerMask for ground detection
    public float maxCastDistance = 20f;     // Maximum cast distance
    public float chargeRate = 10f;          // Rate at which the cast power charges
    public float reelSpeed = 5f;            // Speed of reeling the bobber back

    private float castPower = 0f;           // Current casting power
    private GameObject currentBobber;       // Active bobber in the scene
    private GameObject currentMarker;       // Instance of the cast marker
    public FishingRodState currentState;    // Current state of the rod

    void Start()
    {
        currentState = FishingRodState.Idle;
    }

    void Update()
    {
        switch (currentState)
        {
            case FishingRodState.Idle:
                HandleIdleState();
                break;
            case FishingRodState.Charging:
                HandleChargingState();
                break;
            case FishingRodState.Casting:
                HandleCastingState();
                break;
            case FishingRodState.Reeling:
                HandleReelingState();
                break;
        }
    }

    private void HandleIdleState()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Transition to Charging state
            currentState = FishingRodState.Charging;
            castPower = 0f;

            // Instantiate the marker prefab
            if (castMarkerPrefab != null && currentMarker == null)
            {
                currentMarker = Instantiate(castMarkerPrefab);
                currentMarker.SetActive(true);
            }
        }
    }

    private void HandleChargingState()
    {
        if (Input.GetMouseButton(0))
        {
            // Increase cast power
            castPower += chargeRate * Time.deltaTime;
            castPower = Mathf.Clamp(castPower, 0f, maxCastDistance);

            // Update the marker's position
            if (currentMarker != null)
            {
                Vector3 targetPosition = GetGroundPosition(castPoint.position + castPoint.forward * castPower);
                if (targetPosition != Vector3.zero)
                {
                    currentMarker.transform.position = targetPosition;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            // Transition to Casting state
            currentState = FishingRodState.Casting;

            // Deactivate the marker
            if (currentMarker != null)
            {
                Destroy(currentMarker); // Destroy the marker instance
            }
        }
    }

    private void HandleCastingState()
    {
        CastBobber();
        currentState = FishingRodState.Idle; // Go back to Idle after casting
    }

    private void HandleReelingState()
    {
        if (currentBobber != null)
        {
            StartCoroutine(ReelInCoroutine());
        }
    }

    private void CastBobber()
    {
        if (bobberPrefab != null && castPoint != null)
        {
            currentBobber = Instantiate(bobberPrefab, castPoint.position, Quaternion.identity);

            Vector3 targetPosition = GetGroundPosition(castPoint.position + castPoint.forward * castPower);
            if (targetPosition != Vector3.zero)
            {
                Rigidbody rb = currentBobber.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    Vector3 direction = (targetPosition - castPoint.position).normalized;
                    rb.AddForce(direction * castPower, ForceMode.Impulse);
                }
                else
                {
                    currentBobber.transform.position = targetPosition;
                }
            }

            castPower = 0f; // Reset cast power
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
            Destroy(currentBobber); // Destroy the bobber when it reaches the rod
        }

        // Transition back to Idle state
        currentState = FishingRodState.Idle;
    }

    /// <summary>
    /// Projects a position onto the ground using a raycast.
    /// </summary>
    private Vector3 GetGroundPosition(Vector3 startPosition)
    {
        Ray ray = new Ray(startPosition, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            return hit.point; // Return the point on the ground
        }
        return Vector3.zero; // Return zero if no ground was hit
    }
}
