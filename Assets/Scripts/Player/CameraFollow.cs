using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Follow")]
    [SerializeField] private float smoothTime = 0.25f;        // Higher = more lag, lower = snappier
    [SerializeField] private Vector2 offset = new Vector2(2f, 1f); // Horizontal look-ahead + vertical offset

    [Header("Deadzone")]
    [SerializeField] private Vector2 deadzone = new Vector2(0.5f, 0.2f); // Area where camera doesn't move

    [Header("Bounds")]
    [SerializeField] private bool useBounds = false;
    [SerializeField] private Vector2 minBounds;
    [SerializeField] private Vector2 maxBounds;

    [Header("Vertical Limit")]
    [SerializeField] private float minYPosition = -7f;

    [Header("Left Boundary Slowdown")]
    [SerializeField] private float leftSlowdownX = -7f;      // X position where slowdown starts
    [SerializeField] private float leftSlowdownRange = 3f;    // How many units before the limit the effect starts
    [SerializeField] private float maxSlowdownMultiplier = 6f; // How heavy the camera gets at the limit


    private Vector3 velocity = Vector3.zero;
    private Vector3 targetPosition;
    private PlayerController player;

    private void Awake()
    {
        player = target.GetComponent<PlayerController>();
    }

    private void LateUpdate()
    {
        if (target == null) return;

        CalculateTargetPosition();
        ApplyDeadzone();
        SmoothFollow();
        ApplyBounds();
    }

    private void CalculateTargetPosition()
    {
        // Flip horizontal offset based on player facing direction
        float directionX = player != null && !player.IsFacingRight ? -offset.x : offset.x;

        targetPosition = new Vector3(
            target.position.x + directionX,
            target.position.y + offset.y,
            transform.position.z
        );
    }

    private void ApplyDeadzone()
    {
        float deltaX = targetPosition.x - transform.position.x;
        float deltaY = targetPosition.y - transform.position.y;

        // Only move camera if target exits the deadzone area
        if (Mathf.Abs(deltaX) < deadzone.x) targetPosition.x = transform.position.x;
        if (Mathf.Abs(deltaY) < deadzone.y) targetPosition.y = transform.position.y;
    }

    private void SmoothFollow()
    {
        float currentSmoothTime = smoothTime;

        // Calculate how close the player is to the left boundary
        float distanceToLimit = player.transform.position.x - leftSlowdownX;

        if (distanceToLimit < leftSlowdownRange)
        {
            // Remap distance to a 0-1 value and apply slowdown
            float t = 1f - Mathf.Clamp01(distanceToLimit / leftSlowdownRange);
            currentSmoothTime = Mathf.Lerp(smoothTime, smoothTime * maxSlowdownMultiplier, t);
        }

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref velocity,
            currentSmoothTime
        );
    }
    private void ApplyBounds()
    {
        float clampedY = useBounds
            ? Mathf.Clamp(transform.position.y, minBounds.y, maxBounds.y)
            : Mathf.Max(transform.position.y, minYPosition);

        transform.position = new Vector3(
            useBounds
                ? Mathf.Clamp(transform.position.x, minBounds.x, maxBounds.x)
                : transform.position.x,
            clampedY,
            transform.position.z
        );
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(deadzone.x * 2, deadzone.y * 2, 0));
    }
}