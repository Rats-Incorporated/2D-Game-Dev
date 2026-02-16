using UnityEngine;

public class BearBoss : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    public float stopDistance = 2.5f;

    [Header("Attack Timing")]
    public float attackInterval = 2.5f;   // seconds between attack attempts
    [Range(0f, 1f)]
    public float lungeChance = 0.5f;      // 50/50 by default, tweak in Inspector

    [Header("Lunge Attack")]
    public float lungeHorizontalSpeed = 10f;  // how fast he crosses the ground
    public float lungeJumpForce = 12f;         // upward impulse at launch
    public float lungeCooldownAfter = 0.6f;    // pause after landing

    [Header("Paw Swipe Attack")]
    public Transform swipeHitbox;         // empty child GameObject positioned in front of bear
    public float swipeRadius = 1.2f;
    public float swipeDamage = 20f;
    public float swipeDuration = 0.3f;    // hitbox stays active this long

    [Header("Damage")]
    public float lungeDamage = 30f;

    [Header("Health")]
    public float bossHealth = 300f;
    private float bossCurrentHealth;

    // State machine 
    private enum BossState { Idle, Moving, Lunging, SwipingPaw, Cooldown }
    private BossState state = BossState.Idle;

    private Transform player;
    private Rigidbody2D rb;

    private float attackTimer;
    private float stateTimer;        // general purpose per state countdown
    private float lungeDirectionX;   // -1 or 1, horizontal only
    private bool isGrounded = false;

    //  Swipe hitbox tracking
    private bool swipeActive = false;
    private bool swipeHitRegistered = false;

    // Lunge hit tracking
    private bool lungeHitRegistered = false;

    //  Facing 
    private Vector3 originalScale;

    void Start()
    {
        bossCurrentHealth = bossHealth;
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;

        // Freeze rotation so physics can never tip the bear over
        rb.freezeRotation = true;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        attackTimer = attackInterval;

        // Make sure the swipe hitbox starts invisible / disabled
        if (swipeHitbox != null)
            swipeHitbox.gameObject.SetActive(false);
    }

    void Update()
    {
        if (bossCurrentHealth <= 0) return;
        if (player == null) return;

        // Always face the player
        FacePlayer();

        switch (state)
        {
            case BossState.Idle:
            case BossState.Moving:
                HandleMovementAndTimer();
                break;

            case BossState.Lunging:
                HandleLunge();
                break;

            case BossState.SwipingPaw:
                HandleSwipe();
                break;

            case BossState.Cooldown:
                HandleCooldown();
                break;
        }
    }

    // Facing

    void FacePlayer()
    {
        if (player == null) return;
        float dir = player.position.x - transform.position.x;
        Vector3 s = originalScale;
        s.x = Mathf.Abs(s.x) * (dir < 0 ? -1f : 1f);
        transform.localScale = s;
    }

    // Normal movement + attack selection

    void HandleMovementAndTimer()
    {
        float distance = Mathf.Abs(player.position.x - transform.position.x);

        if (distance > stopDistance)
        {
            state = BossState.Moving;
            float dirX = Mathf.Sign(player.position.x - transform.position.x);
            // Only set X — leave Y alone so gravity keeps him grounded
            rb.linearVelocity = new Vector2(dirX * moveSpeed, rb.linearVelocity.y);
        }
        else
        {
            state = BossState.Idle;
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }

        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0f)
        {
            attackTimer = attackInterval;
            ChooseAttack();
        }
    }

    void ChooseAttack()
    {
        // Stop horizontal movement before attacking, keep Y so gravity still applies
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

        // Weighted coin flip — lungeChance slider controls fairness
        if (Random.value < lungeChance)
            BeginLunge();
        else
            BeginSwipe();
    }

    // Lunge Attack 

    void BeginLunge()
    {
        state = BossState.Lunging;
        lungeDirectionX = Mathf.Sign(player.position.x - transform.position.x);
        lungeHitRegistered = false;

        // Apply horizontal speed + upward impulse, gravity handles the arc
        rb.linearVelocity = new Vector2(lungeDirectionX * lungeHorizontalSpeed, lungeJumpForce);
    }

    void HandleLunge()
    {
        // Keep horizontal speed constant through the air
        rb.linearVelocity = new Vector2(lungeDirectionX * lungeHorizontalSpeed, rb.linearVelocity.y);

        // Land once he's back on the ground after leaving it
        if (isGrounded && rb.linearVelocity.y <= 0f)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            EnterCooldown(lungeCooldownAfter);
        }
    }

    // Any part of the bear touching the player during a lunge deals damage
    void OnCollisionEnter2D(Collision2D collision)
    {
        TryLungeDamage(collision);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        // Covers the case where the player walks under or into the bear mid arc
        TryLungeDamage(collision);
        CheckGrounded(collision);
    }

    void TryLungeDamage(Collision2D collision)
    {
        if (state != BossState.Lunging) return;
        if (lungeHitRegistered) return;
        if (!collision.gameObject.CompareTag("Player")) return;

        PlayerHealth ph = collision.gameObject.GetComponent<PlayerHealth>();
        if (ph != null)
            ph.TakeDamage(lungeDamage);

        lungeHitRegistered = true;

        // Kill horizontal momentum on impact so he doesn't slide through
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // When no contacts remain he's airborne
        isGrounded = false;
    }

    void CheckGrounded(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            // A normal pointing mostly upward means we're standing on something
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                return;
            }
        }
    }

    // ── Paw attack :)

    void BeginSwipe()
    {
        state = BossState.SwipingPaw;
        stateTimer = swipeDuration;
        swipeActive = true;
        swipeHitRegistered = false;

        if (swipeHitbox != null)
            swipeHitbox.gameObject.SetActive(true);
    }

    void HandleSwipe()
    {
        // Check for player overlap while hitbox is active
        if (swipeActive && !swipeHitRegistered && swipeHitbox != null)
        {
            Collider2D hit = Physics2D.OverlapCircle(
                swipeHitbox.position,
                swipeRadius,
                LayerMask.GetMask("Player")   // make sure your Player layer is set
            );

            if (hit != null && hit.CompareTag("Player"))
            {
                PlayerHealth ph = hit.GetComponent<PlayerHealth>();
                if (ph != null)
                    ph.TakeDamage(swipeDamage);

                swipeHitRegistered = true;   // one hit per swing
            }
        }

        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0f)
        {
            swipeActive = false;
            if (swipeHitbox != null)
                swipeHitbox.gameObject.SetActive(false);

            EnterCooldown(0f);   // no extra cooldown for swipe
        }
    }

    // Cooldown / recovery

    void EnterCooldown(float duration)
    {
        state = BossState.Cooldown;
        stateTimer = duration;
    }

    void HandleCooldown()
    {
        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0f)
        {
            state = BossState.Moving;
            attackTimer = attackInterval;   // reset so he doesn't instantly attack again
        }
    }

    // Gizmos 

    void OnDrawGizmosSelected()
    {
        // Visualise swipe range in Scene view
        if (swipeHitbox != null)
        {
            Gizmos.color = new Color(1f, 0.3f, 0f, 0.4f);
            Gizmos.DrawWireSphere(swipeHitbox.position, swipeRadius);
        }

        // Visualise stop distance
        Gizmos.color = new Color(0f, 1f, 0f, 0.25f);
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }

    // Health

    public void BossTakeDamage(float amount)
    {
        bossCurrentHealth -= amount;
        bossCurrentHealth = Mathf.Clamp(bossCurrentHealth, 0f, bossHealth);

        if (bossCurrentHealth <= 0f)
            Die();
    }

    void Die()
    {
        if (swipeHitbox != null)
            swipeHitbox.gameObject.SetActive(false);

        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        Destroy(gameObject);
    }
}