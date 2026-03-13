using UnityEngine;

public class BearBoss : MonoBehaviour
{
    [Header("Arena Bounds")]
    public float arenaMinX = -10f;
    public float arenaMaxX = 10f;
    public float moveSpeed = 2f;
    public float stopDistance = 2.5f;

    [Header("Too-Close Shove")]
    public float shoveDistance = 1.4f;         // how close the player needs to be
    public float shoveLingerTime = 0.5f;       // how long they have to stay that close before getting blasted
    public float shoveForce = 14f;
    public float shoveUpForce = 6f;
    public float shoveCooldown = 1.2f;         // gap before it can trigger again
    private float shoveLingerTimer = 0f;
    private float shoveCooldownTimer = 0f;

    [Header("Damage Cooldown")]
    public float damageCooldown = 0.3f;   // min time between incoming hits — breaks button spam
    private float damageCooldownTimer = 0f;

    [Header("Attack Timing")]
    public float attackInterval = 1.5f;
    [Range(0f, 1f)]
    public float lungeChance = 0.5f;

    [Header("Lunge Attack")]
    public float lungeHorizontalSpeed = 10f;
    public float lungeJumpForce = 12f;
    public float lungeCooldownAfter = 0.6f;

    [Header("Paw Swipe Attack")]
    public Transform swipeHitbox;
    public float swipeRadius = 1.2f;
    public float swipeDamage = 20f;
    public float swipeDuration = 0.3f;

    [Header("Damage")]
    public float lungeDamage = 30f;

    [Header("Player Knockback")]
    public float hitKnockbackForce = 6f;
    public float hitKnockbackUp = 3f;

    [Header("Stuck / Back-jump")]
    public float stuckTimeThreshold = 2f;
    public float stuckMoveThreshold = 0.05f;
    public float backJumpHorizontalSpeed = 6f;
    public float backJumpForce = 9f;

    [Header("Health")]
    public float bossHealth = 300f;
    private float bossCurrentHealth;

    // State machine
    private enum BossState { Idle, Moving, Lunging, SwipingPaw, Cooldown, BackJump }
    private BossState state = BossState.Idle;

    private Transform player;
    private Rigidbody2D rb;

    private float attackTimer;
    private float stateTimer;
    private float lungeDirectionX;
    private bool isGrounded = false;

    private bool swipeActive = false;
    private bool swipeHitRegistered = false;
    private bool lungeHitRegistered = false;

    private float stuckTimer = 0f;
    private Vector2 lastPosition;
    private float backJumpDirectionX;

    private Animator anim;

    private Vector3 originalScale;

    void Start()
    {
        bossCurrentHealth = bossHealth;
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
        rb.freezeRotation = true;
        anim = GetComponent<Animator>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        attackTimer = attackInterval;
        lastPosition = rb.position;

        if (swipeHitbox != null)
            swipeHitbox.gameObject.SetActive(false);
    }

    void Update()
    {
        if (bossCurrentHealth <= 0) return;
        if (player == null) return;

        FacePlayer();

        shoveCooldownTimer -= Time.deltaTime;
        damageCooldownTimer -= Time.deltaTime;

        switch (state)
        {
            case BossState.Idle:
            case BossState.Moving:
                HandleMovementAndTimer();
                TrackStuck();
                CheckTooClose();
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

            case BossState.BackJump:
                HandleBackJump();
                break;
        }

        lastPosition = rb.position;

        // Keep bear inside arena bounds
        if (rb.position.x < arenaMinX || rb.position.x > arenaMaxX)
        {
            Vector2 clamped = rb.position;
            clamped.x = Mathf.Clamp(rb.position.x, arenaMinX, arenaMaxX);
            rb.position = clamped;
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
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

    // Too close shove

    void CheckTooClose()
    {
        if (shoveCooldownTimer > 0f)
        {
            shoveLingerTimer = 0f;   // don't accumulate while on cooldown
            return;
        }

        float distance = Mathf.Abs(player.position.x - transform.position.x);

        if (distance < shoveDistance)
        {
            shoveLingerTimer += Time.deltaTime;

            if (shoveLingerTimer >= shoveLingerTime)
            {
                shoveLingerTimer = 0f;
                shoveCooldownTimer = shoveCooldown;
                KnockbackPlayer(player.gameObject, shoveForce, shoveUpForce);

                // Also immediately trigger an attack so the bear follows up after the shove
                attackTimer = 0f;
            }
        }
        else
        {
            shoveLingerTimer = 0f;   // reset if they back off
        }
    }

    // Stuck detection

    void TrackStuck()
    {
        float moved = Mathf.Abs(rb.position.x - lastPosition.x);

        if (moved < stuckMoveThreshold)
            stuckTimer += Time.deltaTime;
        else
            stuckTimer = 0f;

        if (stuckTimer >= stuckTimeThreshold)
        {
            stuckTimer = 0f;
            BeginBackJump();
        }
    }

    // Normal movement + attack selection

    void HandleMovementAndTimer()
    {
        float distance = Mathf.Abs(player.position.x - transform.position.x);

        if (distance > stopDistance)
        {
            state = BossState.Moving;
            float dirX = Mathf.Sign(player.position.x - transform.position.x);
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
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

        if (Random.value < lungeChance)
            BeginLunge();
        else
            BeginSwipe();
    }

    //  Back jump (unstuck)

    void BeginBackJump()
    {
        state = BossState.BackJump;
        backJumpDirectionX = -Mathf.Sign(player.position.x - transform.position.x);
        rb.linearVelocity = new Vector2(backJumpDirectionX * backJumpHorizontalSpeed, backJumpForce);
    }

    void HandleBackJump()
    {
        rb.linearVelocity = new Vector2(backJumpDirectionX * backJumpHorizontalSpeed, rb.linearVelocity.y);

        if (isGrounded && rb.linearVelocity.y <= 0f)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            EnterCooldown(0.3f);
        }
    }

    // Lunge Attack

    void BeginLunge()
    {
        state = BossState.Lunging;
        lungeDirectionX = Mathf.Sign(player.position.x - transform.position.x);
        lungeHitRegistered = false;
        rb.linearVelocity = new Vector2(lungeDirectionX * lungeHorizontalSpeed, lungeJumpForce);
    }

    void HandleLunge()
    {
        rb.linearVelocity = new Vector2(lungeDirectionX * lungeHorizontalSpeed, rb.linearVelocity.y);

        if (isGrounded && rb.linearVelocity.y <= 0f)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            EnterCooldown(lungeCooldownAfter);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        CheckGrounded(collision);
        TryLungeDamage(collision);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        CheckGrounded(collision);
        TryLungeDamage(collision);
    }

    void TryLungeDamage(Collision2D collision)
    {
        if (state != BossState.Lunging) return;
        if (lungeHitRegistered) return;
        if (!collision.gameObject.CompareTag("Player")) return;

        PlayerHealth ph = collision.gameObject.GetComponent<PlayerHealth>();
        if (ph != null)
            ph.TakeDamage(lungeDamage);

        KnockbackPlayer(collision.gameObject, hitKnockbackForce, hitKnockbackUp);

        lungeHitRegistered = true;
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }

    void CheckGrounded(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                return;
            }
        }
    }

    // Player knockback helper 

    void KnockbackPlayer(GameObject playerObj, float horizontal, float vertical)
    {
        Rigidbody2D playerRb = playerObj.GetComponent<Rigidbody2D>();
        if (playerRb == null) return;

        // Bear always faces the player so localScale.x tells us which way is "away"
        float awayDir = Mathf.Sign(transform.localScale.x);

        playerRb.linearVelocity = Vector2.zero;   // clear current momentum first
        playerRb.AddForce(new Vector2(awayDir * horizontal, vertical), ForceMode2D.Impulse);
    }

    // Paw Swipe Attack 

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
        if (swipeActive && !swipeHitRegistered && swipeHitbox != null)
        {
            Collider2D hit = Physics2D.OverlapCircle(
                swipeHitbox.position,
                swipeRadius,
                LayerMask.GetMask("Player")
            );

            if (hit != null && hit.CompareTag("Player"))
            {
                PlayerHealth ph = hit.GetComponent<PlayerHealth>();
                if (ph != null)
                    ph.TakeDamage(swipeDamage);

                KnockbackPlayer(hit.gameObject, hitKnockbackForce, hitKnockbackUp);
                swipeHitRegistered = true;
            }
        }

        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0f)
        {
            swipeActive = false;
            if (swipeHitbox != null)
                swipeHitbox.gameObject.SetActive(false);

            EnterCooldown(0f);
        }
    }

    //  Cooldown / recovery

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
            attackTimer = attackInterval;
        }
    }

    // Gizmos 

    void OnDrawGizmosSelected()
    {
        if (swipeHitbox != null)
        {
            Gizmos.color = new Color(1f, 0.3f, 0f, 0.4f);
            Gizmos.DrawWireSphere(swipeHitbox.position, swipeRadius);
        }

        Gizmos.color = new Color(0f, 1f, 0f, 0.25f);
        Gizmos.DrawWireSphere(transform.position, stopDistance);

        Gizmos.color = new Color(1f, 0f, 0f, 0.25f);
        Gizmos.DrawWireSphere(transform.position, shoveDistance);
    }

    //  Health

    public void BossTakeDamage(float amount)
    {
        if (damageCooldownTimer > 0f) return;

        damageCooldownTimer = damageCooldown;
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