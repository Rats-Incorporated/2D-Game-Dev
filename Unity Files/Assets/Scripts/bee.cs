using UnityEngine;
using System.Collections;

public class MovingEnemyPlatform : MonoBehaviour
{
    // --- Movement Settings (from MovingPlatform) ---
    [Header("Movement Settings")]
    [SerializeField] private Vector2 startPosition;
    [SerializeField] private Vector2 endPosition;
    [SerializeField] private float speed = 2f;

    [Header("Setup")]
    [SerializeField] private bool setStartPositionOnAwake = true;

    // --- Enemy Settings (from EnemyDefault) ---
    [Header("Enemy Settings")]
    public float damageAmount = 25f;
    public float enemyHealth = 50f;

    // --- Private/State Variables ---
    private Vector2 targetPosition;
    private bool movingToEnd = true;
    private float enemyCurrentHealth;
    private Animator animator;
    private Vector3 originalScale;

    private void Awake()
    {
        // Set up movement
        if (setStartPositionOnAwake)
        {
            startPosition = transform.position;
        }
        targetPosition = endPosition;

        // Set up enemy
        enemyCurrentHealth = enemyHealth;
        animator = GetComponent<Animator>();
        originalScale = transform.localScale;
    }

    private void Update()
    {
        // Handle death check
        if (enemyCurrentHealth <= 0)
        {
            return;
        }

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

        // Flip sprite based on movement direction
        if (targetPosition.x < transform.position.x)
        {
            transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
        }
        else if (targetPosition.x > transform.position.x)
        {
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
        }
    }

    // --- Player Damage System ---
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth player = collision.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(damageAmount);
            }
        }
    }

    // --- Health and Damage Functions ---
    public void EnemyTakeDamage(float amount)
    {
        enemyCurrentHealth -= amount;
        enemyCurrentHealth = Mathf.Clamp(enemyCurrentHealth, 0, enemyHealth);

        if (enemyCurrentHealth <= 0)
        {
            Destroy(gameObject);
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