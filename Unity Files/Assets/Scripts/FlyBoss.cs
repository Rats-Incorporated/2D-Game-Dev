using UnityEngine;
using System.Collections;

public class FlyBoss : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    public float stopDistance = 3f;

    [Header("Arena Bounds")]
    public float minX = -10f;
    public float maxX = 10f;

    [Header("Attack")]
    public GameObject projectilePrefab;
    public Transform firePointLeft;
    public Transform firePointRight;
    public float fireInterval = 1.5f;
    public float projectileSpeed = 5f;

    [Header("Slam Attack")]
    public GameObject slamProjectilePrefab;
    public Transform slamFirePointLeft;
    public Transform slamFirePointRight;
    public float slamCooldown = 6f;
    public float slamSpeed = 8f;

    [Header("Health")]
    public float bossHealth = 200f;

    [Header("Contact Damage")]
    public float contactDamage = 15f;
    public float contactDamageCooldown = 1f;

    private float bossCurrentHealth;
    private Transform player;
    private float fireTimer;
    private float slamTimer;

    private bool isSlamming = false;
    private float originalY;

    private float contactTimer;

    void Start()
    {
        bossCurrentHealth = bossHealth;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        fireTimer = fireInterval;
        slamTimer = slamCooldown;
        originalY = transform.position.y;
    }

    void Update()
    {
        if (bossCurrentHealth <= 0) return;
        if (player == null) return;
        if (isSlamming) return;

        contactTimer -= Time.deltaTime;

        HandleMovement();
        HandleAttack();
        HandleSlamTimer();
        ClampPosition();
    }

    // MOVEMENT 

    void HandleMovement()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > stopDistance)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
        }
    }

    void ClampPosition()
    {
        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }

    //  NORMAL ATTACK 

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
        ShootFromFirePoint(firePointLeft, projectilePrefab);
        ShootFromFirePoint(firePointRight, projectilePrefab);
    }

    // SLAM TIMER 

    void HandleSlamTimer()
    {
        slamTimer -= Time.deltaTime;

        if (slamTimer <= 0f)
        {
            StartCoroutine(SlamAttack());
            slamTimer = slamCooldown;
        }
    }

    IEnumerator SlamAttack()
    {
        isSlamming = true;

        // Move downward until hitting Terrain
        while (true)
        {
            transform.position += Vector3.down * slamSpeed * Time.deltaTime;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.5f);
            if (hit.collider != null && hit.collider.CompareTag("Terrain"))
            {
                break;
            }

            yield return null;
        }

        // Fire wave projectiles
        ShootFromFirePoint(slamFirePointLeft, slamProjectilePrefab);
        ShootFromFirePoint(slamFirePointRight, slamProjectilePrefab);

        yield return new WaitForSeconds(0.5f);

        // Move back up to original height
        while (transform.position.y < originalY)
        {
            transform.position += Vector3.up * slamSpeed * Time.deltaTime;
            yield return null;
        }

        transform.position = new Vector3(transform.position.x, originalY, transform.position.z);

        isSlamming = false;
    }

    //  SHOOT FUNCTION

    void ShootFromFirePoint(Transform firePoint, GameObject projectileType)
    {
        if (firePoint == null || projectileType == null) return;

        GameObject projectile = Instantiate(
            projectileType,
            firePoint.position,
            firePoint.rotation
        );

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            float speedToUse = projectileType == slamProjectilePrefab ? slamSpeed : projectileSpeed;
            rb.linearVelocity = firePoint.right * speedToUse;
        }

        Collider2D projCol = projectile.GetComponent<Collider2D>();
        Collider2D bossCol = GetComponent<Collider2D>();
        if (projCol != null && bossCol != null)
        {
            Physics2D.IgnoreCollision(projCol, bossCol);
        }
    }

    // HEALTH 

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

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (contactTimer > 0f) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(contactDamage);
                contactTimer = contactDamageCooldown;
            }
        }
    }
}