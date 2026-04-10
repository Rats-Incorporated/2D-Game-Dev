using System.Collections;
using UnityEngine;

public class EnemyLauncher : MonoBehaviour
{
    // Public Parameters
    public float damageAmount = 25f;
    public float enemyHealth = 50f;
    public float moveSpeed = 3f;
    public float chaseRange = 35f;
    public float stopRange = 15f;
    public LayerMask playerLayer;
    public float patrolMinX = -10f;
    public float patrolMaxX = 10f;

    [Header("Launch Settings")]
    public float launchForceX = 20f;   // Horizontal knockback strength
    public float launchForceY = 15f;   // Vertical launch strength
    public float launchCooldown = 0.4f;

    [Header("Jump Settings")]
    public float jumpForce = 10f;
    public float jumpInterval = 2f;

    DamageFlash flash;

    //  Private/State Variables 
    private Animator animator;
    private float enemyCurrentHealth;
    private Transform playerTransform;
    private Vector2 currentWanderTarget;
    private Vector3 originalScale;
    private float launchTimer;
    private float jumpTimer;
    private Rigidbody2D rb;

    private enum EnemyState { Wander, Chase }
    private EnemyState currentState = EnemyState.Wander;

    void Start()
    {
        enemyCurrentHealth = enemyHealth;
        animator = GetComponent<Animator>();
        flash = GetComponent<DamageFlash>();
        rb = GetComponent<Rigidbody2D>();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
            playerTransform = playerObject.transform;

        SetNewWanderTarget();
        originalScale = transform.localScale;

        // Start timer as ready to launch immediately
        launchTimer = launchCooldown;
        jumpTimer = jumpInterval;
    }

    void Update()
    {
        if (enemyCurrentHealth <= 0) return;

        // Tick the launch cooldown up
        if (launchTimer < launchCooldown)
            launchTimer += Time.deltaTime;

        jumpTimer -= Time.deltaTime;
        if (jumpTimer <= 0f)
        {
            Jump();
            jumpTimer = jumpInterval;
        }

        CheckForPlayer();

        switch (currentState)
        {
            case EnemyState.Wander: WanderBehavior(); break;
            case EnemyState.Chase: ChaseBehavior(); break;
        }
    }

    //  Player Detection

    void CheckForPlayer()
    {
        if (playerTransform == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= chaseRange)
        {
            currentState = EnemyState.Chase;
        }
        else
        {
            if (currentState != EnemyState.Wander)
            {
                currentState = EnemyState.Wander;
                SetNewWanderTarget();
            }
        }
    }

    //  State Behaviors 

    void WanderBehavior()
    {
        Vector2 targetPosition = new Vector2(currentWanderTarget.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, patrolMinX, patrolMaxX),
            transform.position.y,
            transform.position.z
        );

        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
            SetNewWanderTarget();

        FlipSprite(targetPosition.x);
    }

    void ChaseBehavior()
    {
        if (playerTransform == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        Vector2 targetPosition = new Vector2(playerTransform.position.x, transform.position.y);

        if (distanceToPlayer > stopRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, patrolMinX, patrolMaxX),
                transform.position.y,
                transform.position.z
            );
        }

        FlipSprite(playerTransform.position.x);
    }

    // Launch on Contact 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        // Damage
        PlayerHealth player = collision.GetComponent<PlayerHealth>();
        if (player != null)
            player.TakeDamage(damageAmount);

        // Launch — only if cooldown has elapsed
        if (launchTimer >= launchCooldown)
        {
            launchTimer = 0f;

            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Figure out which direction to knock the player (away from enemy)
                float direction = collision.transform.position.x > transform.position.x ? 1f : -1f;

                // Zero out current velocity so the launch feels snappy and consistent
                rb.linearVelocity = Vector2.zero;

                rb.AddForce(new Vector2(direction * launchForceX, launchForceY), ForceMode2D.Impulse);
            }
        }
    }

    //  Health and Damage

    public void EnemyTakeDamage(float amount)
    {
        enemyCurrentHealth -= amount;
        enemyCurrentHealth = Mathf.Clamp(enemyCurrentHealth, 0, enemyHealth);

        if (flash != null)
            flash.Flash();

        if (enemyCurrentHealth <= 0)
            Destroy(gameObject);
    }

    // Helpers

    void Jump()
    {
        if (rb == null) return;
        rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
    }

    void SetNewWanderTarget()
    {
        float wanderRadius = 5f;
        float randomX = Random.Range(-wanderRadius, wanderRadius);
        float clampedX = Mathf.Clamp(transform.position.x + randomX, patrolMinX, patrolMaxX);
        currentWanderTarget = new Vector2(clampedX, transform.position.y);
    }

    void FlipSprite(float targetX)
    {
        if (targetX < transform.position.x)
            transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
        else if (targetX > transform.position.x)
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
    }
}