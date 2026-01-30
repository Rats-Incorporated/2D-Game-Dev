using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private Vector2 startPosition;
    [SerializeField] private Vector2 endPosition;
    [SerializeField] private float speed = 2f;

    [Header("Setup")]
    [SerializeField] private bool setStartPositionOnAwake = true;

    private Vector2 targetPosition;
    private bool movingToEnd = true;

    private void Awake()
    {
        // Optionally set the start position to the platform's current position
        if (setStartPositionOnAwake)
        {
            startPosition = transform.position;
        }

        targetPosition = endPosition;
    }

    private void Update()
    {
        // Move the platform towards the target position
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Check if we've reached the target position
        if (Vector2.Distance(transform.position, targetPosition) < 0.01f)
        {
            // Switch direction
            if (movingToEnd)
            {
                targetPosition = startPosition;
                movingToEnd = false;
            }
            else
            {
                targetPosition = endPosition;
                movingToEnd = true;
            }
        }
    }

    // Visual helper in the Unity editor to see the platform path
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(startPosition, endPosition);
        Gizmos.DrawWireSphere(startPosition, 0.3f);
        Gizmos.DrawWireSphere(endPosition, 0.3f);
    }
}