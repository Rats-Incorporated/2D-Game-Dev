using System.Collections;
using UnityEngine;

public class EnemySlap : MonoBehaviour
{
    public float damageAmount = 25f;
    public float enemyHealth = 50f;

    public float moveSpeed = 3f;
    public float chaseRange = 35f;
    public float stopRange = 4f;

    public float slapForce = 20f;
    public float attackCooldown = 3f;

    public float patrolMinX = -10f;
    public float patrolMaxX = 10f;

    private Animator animator;
    private Transform playerTransform;
    private float enemyCurrentHealth;

    private float lastAttackTime;

    private Vector2 currentWanderTarget;
    private Vector3 originalScale;

    DamageFlash flash;

    private enum EnemyState { Wander, Chase, Attack }
    private EnemyState currentState = EnemyState.Wander;

    void Start()
    {
        animator = GetComponent<Animator>();
        flash = GetComponent<DamageFlash>();

        enemyCurrentHealth = enemyHealth;

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
            playerTransform = playerObject.transform;

        SetNewWanderTarget();
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (enemyCurrentHealth <= 0) return;

        CheckForPlayer();

        switch (currentState)
        {
            case EnemyState.Wander:
                WanderBehavior();
                break;

            case EnemyState.Chase:
                ChaseBehavior();
                break;

            case EnemyState.Attack:
                // Wait for animation event
                break;
        }
    }

    void CheckForPlayer()
    {
        if (playerTransform == null) return;

        float distance = Vector2.Distance(transform.position, playerTransform.position);

        if (distance <= stopRange && Time.time > lastAttackTime + attackCooldown)
        {
            currentState = EnemyState.Attack;
            animator.SetTrigger("Slap");
        }
        else if (distance <= chaseRange)
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

    void WanderBehavior()
    {
        // Unity has been complaining about there being no "Walk" parameter
        // commenting this out for now
        // animator.SetBool("Walk", true);

        Vector2 target = new Vector2(currentWanderTarget.x, transform.position.y);

        transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, patrolMinX, patrolMaxX),
            transform.position.y,
            transform.position.z
        );

        if (Vector2.Distance(transform.position, target) < 0.1f)
            SetNewWanderTarget();

        Flip(target.x);
    }

    void ChaseBehavior()
    {
        // same as above
        // animator.SetBool("Walk", true);

        Vector2 target = new Vector2(playerTransform.position.x, transform.position.y);

        transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, patrolMinX, patrolMaxX),
            transform.position.y,
            transform.position.z
        );

        Flip(playerTransform.position.x);
    }

    void Flip(float targetX)
    {
        if (targetX < transform.position.x)
            transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
        else
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
    }

    void SetNewWanderTarget()
    {
        float wanderRadius = 5f;

        float randomX = Random.Range(-wanderRadius, wanderRadius);

        float clampedX = Mathf.Clamp(transform.position.x + randomX, patrolMinX, patrolMaxX);

        currentWanderTarget = new Vector2(clampedX, transform.position.y);
    }

    // Called by Animation Event during slap
    public void DoSlapDamage()
    {
        if (playerTransform == null) return;

        float distance = Vector2.Distance(transform.position, playerTransform.position);

        if (distance <= stopRange + 1f)
        {
            PlayerHealth player = playerTransform.GetComponent<PlayerHealth>();
            Rigidbody2D rb = playerTransform.GetComponent<Rigidbody2D>();

            if (player != null)
                player.TakeDamage(damageAmount);

            if (rb != null)
            {
                Vector2 direction = (playerTransform.position - transform.position).normalized;

                rb.AddForce(direction * slapForce, ForceMode2D.Impulse);
            }
        }

        lastAttackTime = Time.time;
        currentState = EnemyState.Chase;
    }

    public void EnemyTakeDamage(float amount)
    {
        enemyCurrentHealth -= amount;
        enemyCurrentHealth = Mathf.Clamp(enemyCurrentHealth, 0, enemyHealth);

        if (flash != null)
            flash.Flash();

        if (enemyCurrentHealth <= 0)
            Destroy(gameObject);
    }
}
