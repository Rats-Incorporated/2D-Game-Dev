using UnityEngine;
using System.Collections;

public class EnemyDefault : MonoBehaviour
{
    // --- Public Parameters ---
    public float damageAmount = 25f;
    public float enemyHealth = 50f;
    public float moveSpeed = 3f; // Speed for chasing/wandering
    public float chaseRange = 35f; // Distance for long-range player detection (Raycast length)
    public float stopRange = 15f;
    public LayerMask playerLayer; // Assign the Layer that your Player is on in the Inspector

    // --- Private/State Variables ---
    private Animator animator;
    private float enemyCurrentHealth;
    private Transform playerTransform; // Reference to the player's transform
    private Vector2 currentWanderTarget;
    private Vector3 originalScale;

    // Defines the different behaviors the enemy can have
    private enum EnemyState { Wander, Chase }
    private EnemyState currentState = EnemyState.Wander;

    void Start()
    {
        enemyCurrentHealth = enemyHealth;
        animator = GetComponent<Animator>();
        // Find the player object and store its transform
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
        SetNewWanderTarget();
        originalScale = transform.localScale;
        animator.SetBool("Walk", false); 
    }

    // Update is called once per frame
   void Update()
    {
        // Handle death check
        if (enemyCurrentHealth <= 0)
        {
            // The Destroy(gameObject); is still in EnemyTakeDamage, but a death animation/logic can go here
            return;
        }

        // --- Core Enemy Logic based on State ---

        // 1. Raycast Check (for state transitions)
        CheckForPlayer();
        Debug.Log("Current State: " + currentState.ToString());

        // 2. Execute Current State Behavior
        switch (currentState)
        {
            case EnemyState.Wander:
                WanderBehavior();
                break;
            case EnemyState.Chase:
                ChaseBehavior();
                break;
        }
    }
    
   // --- Player Detection and State Logic ---

    void CheckForPlayer()
    {
        if (playerTransform == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        Debug.Log("Distance to Player: " + distanceToPlayer.ToString("F2") + " units");
        
        // Use direct distance check instead of Raycast, as it's simpler for this 2D behavior
        // If you still prefer the Raycast for line-of-sight, you can replace the distance check.

        if (distanceToPlayer <= chaseRange)
        {
            // Player is within 5 units: Chase
            currentState = EnemyState.Chase;
        }
        else
        {
            // Player is outside 5 units: Wander
            if (currentState != EnemyState.Wander)
            {
                currentState = EnemyState.Wander;
                SetNewWanderTarget(); 
            }
        }
    }

    // --- State Behaviors ---

    void WanderBehavior()
    {
        // Lock Y position
        Vector2 targetPosition = new Vector2(currentWanderTarget.x, transform.position.y); 

        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Animation: Always walking in Wander state
        animator.SetBool("Walk", false); 

        // Check if the enemy has reached the target
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            SetNewWanderTarget(); 
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

    void ChaseBehavior()
    {
        if (playerTransform == null) return;
        
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        
        // Lock Y position
        Vector2 targetPosition = new Vector2(playerTransform.position.x, transform.position.y);

        if (distanceToPlayer > stopRange) // stopRange is 1 unit
        {
            // Player is farther than 1 unit: Chase
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            animator.SetBool("Walk", false); // Run animation
        }
        else 
        {
            // Player is within 1 unit: Stop/Idle 
            // The damage will be handled by OnTriggerEnter2D
            animator.SetBool("Walk", true); // Idle animation
            // NOTE: Add your specific close-range/stop animation logic here if different from Idle.
        }
        
        // Flip sprite to face the player (only check X position)
        if (playerTransform.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
        }
        else if (playerTransform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
        }
    }

    // --- Helper Functions ---

    void SetNewWanderTarget()
    {
        float wanderRadius = 5f;
        float randomX = Random.Range(-wanderRadius, wanderRadius); 
        currentWanderTarget = new Vector2(transform.position.x + randomX, transform.position.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // This is generally used for continuous damage or a simple collision attack.
            // For a deliberate attack sequence, DoDamageToPlayer is better.
            PlayerHealth player = collision.GetComponent<PlayerHealth>();

            player.TakeDamage(damageAmount);


        }
    }

    // --- Health and Damage Functions ---

    public void EnemyTakeDamage(float amount)
    {
        enemyCurrentHealth -= amount;
        enemyCurrentHealth = Mathf.Clamp(enemyCurrentHealth, 0, enemyHealth); // prevents negative damage

        // Check if enemy health is low to trigger a specific animation/behavior (if needed)
        // Your original logic: if (enemyCurrentHealth < 26) { animator.SetBool("Walk", true); }
        // I've removed this from Update, but you can re-add low-health behavior here if desired.

        if (enemyCurrentHealth <= 0)
        {
            Destroy(gameObject); // Enemy dies and is removed
        }
    }
}
