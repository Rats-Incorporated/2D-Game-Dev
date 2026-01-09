using UnityEngine;

public class FlyBoss : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    public float stopDistance = 3f;

    [Header("Attack")]
    public GameObject projectilePrefab;
    public Transform firePointLeft;
    public Transform firePointRight;
    public float fireInterval = 1.5f;
    public float projectileSpeed = 5f;

    [Header("Health")]
    public float bossHealth = 200f;

    private float bossCurrentHealth;
    private Transform player;
    private float fireTimer;

    void Start()
    {
        bossCurrentHealth = bossHealth;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        fireTimer = fireInterval;
    }

    void Update()
    {
        if (bossCurrentHealth <= 0) return;
        if (player == null) return;

        HandleMovement();
        HandleAttack();
    }

    // movement

    void HandleMovement()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > stopDistance)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
        }
    }

    // attack

    void HandleAttack()
    {
        fireTimer -= Time.deltaTime;

        if (fireTimer <= 0f)
        {
            ShootBothSides();
            fireTimer = fireInterval;
        }
    }

    void ShootBothSides()
    {
        ShootFromFirePoint(firePointLeft);
        ShootFromFirePoint(firePointRight);
    }

    void ShootFromFirePoint(Transform firePoint)
    {
        if (firePoint == null || projectilePrefab == null) return;

        GameObject projectile = Instantiate(
            projectilePrefab,
            firePoint.position,
            firePoint.rotation
        );

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = firePoint.right * projectileSpeed;
        }

        // Ignore boss for attack
        Collider2D projCol = projectile.GetComponent<Collider2D>();
        Collider2D bossCol = GetComponent<Collider2D>();
        if (projCol != null && bossCol != null)
        {
            Physics2D.IgnoreCollision(projCol, bossCol);
        }
    }

    // health

    public void BossTakeDamage(float amount)
    {
        bossCurrentHealth -= amount;
        bossCurrentHealth = Mathf.Clamp(bossCurrentHealth, 0, bossHealth);

        if (bossCurrentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
