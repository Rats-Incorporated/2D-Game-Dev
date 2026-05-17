using System.Collections;
using UnityEngine;

public class ScorpionBoss : MonoBehaviour
{
    //  Public Parameters 
    public float damageAmount = 40f;
    public float bossHealth = 300f;
    public float moveSpeed = 2.5f;
    public float chaseRange = 40f;
    public float stopRange = 12f;
    public LayerMask playerLayer;
    public float patrolMinX = -15f;
    public float patrolMaxX = 15f;

    [Header("Contact Launch Settings")]
    public float launchForceX = 25f;
    public float launchForceY = 18f;
    public float launchCooldown = 0.5f;

    [Header("Jump Settings")]
    public float jumpForce = 12f;
    public float jumpInterval = 3f;

    [Header("Sting Attack Settings")]
    public float stingDamage = 60f;
    public Transform stingHitboxOrigin;
    public float stingHitboxRadius = 2f;
    public float stingAttackRange = 14f;
    public float stingCooldown = 4f;

    [Header("Animation Names")]
    public string tailShakeAnim = "TailShake";
    public string stingAnim = "Sting";
    public string walkAnim = "Walk";
    public string idleAnim = "Idle";

    // Private / State 
    private Animator animator;
    private Rigidbody2D rb;
    private DamageFlash flash;

    private float currentHealth;
    private Transform playerTransform;
    private Vector2 currentWanderTarget;
    private Vector3 originalScale;

    private float launchTimer;
    private float jumpTimer;
    private float stingTimer;

    private bool isAttacking = false;

    private enum BossState { Wander, Chase }
    private BossState currentState = BossState.Wander;

    //  Unity Lifecycle 
    void Start()
    {
        currentHealth = bossHealth;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        flash = GetComponent<DamageFlash>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) playerTransform = playerObj.transform;

        SetNewWanderTarget();
        originalScale = transform.localScale;

        launchTimer = launchCooldown;
        jumpTimer = jumpInterval;
        stingTimer = stingCooldown;
        
        stingTimer = 0f;
    }

    void Update()
    {
        if (currentHealth <= 0) return;
        if (isAttacking) return;

        if (launchTimer < launchCooldown) launchTimer += Time.deltaTime;

        // Only tick stingTimer when NOT attacking, so cooldown starts AFTER the attack finishes
        if (!isAttacking) stingTimer += Time.deltaTime;

        jumpTimer -= Time.deltaTime;
        if (jumpTimer <= 0f)
        {
            Jump();
            jumpTimer = jumpInterval;
        }

        CheckForPlayer();

        if (currentState == BossState.Chase && TryStingAttack()) return;

        switch (currentState)
        {
            case BossState.Wander: WanderBehavior(); break;
            case BossState.Chase: ChaseBehavior(); break;
        }
    }

    // Player Detection 
    void CheckForPlayer()
    {
        if (playerTransform == null) return;

        float dist = Vector2.Distance(transform.position, playerTransform.position);

        if (dist <= chaseRange)
        {
            currentState = BossState.Chase;
        }
        else if (currentState != BossState.Wander)
        {
            currentState = BossState.Wander;
            SetNewWanderTarget();
        }
    }

    // State Behaviors
    void WanderBehavior()
    {
        animator.Play(walkAnim);

        Vector2 targetPos = new Vector2(currentWanderTarget.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        transform.position = ClampX(transform.position);

        if (Vector2.Distance(transform.position, targetPos) < 0.1f)
            SetNewWanderTarget();

        FlipSprite(targetPos.x);
    }

    void ChaseBehavior()
    {
        if (playerTransform == null) return;

        float dist = Vector2.Distance(transform.position, playerTransform.position);
        Vector2 targetPos = new Vector2(playerTransform.position.x, transform.position.y);

        if (dist > stopRange)
        {
            animator.Play(walkAnim);
            transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            transform.position = ClampX(transform.position);
        }
        else
        {
            animator.Play(idleAnim);
        }

        FlipSprite(playerTransform.position.x);
    }

    bool TryStingAttack()
    {
        if (playerTransform == null) return false;
        if (isAttacking) return false;
        if (stingTimer < stingCooldown) return false;

        float dist = Vector2.Distance(transform.position, playerTransform.position);
        if (dist > stingAttackRange) return false;

        stingTimer = 0f;
        StartCoroutine(StingSequence());
        return true;
    }

    IEnumerator StingSequence()
    {
        isAttacking = true;
        stingTimer = -stingCooldown; // Must climb a full cooldown's worth before firing again

        if (playerTransform != null)
            FlipSprite(playerTransform.position.x);

        for (int i = 0; i < 3; i++)
        {
            animator.Play(tailShakeAnim);
            yield return new WaitForSeconds(GetAnimationLength(tailShakeAnim));
        }

        animator.Play(stingAnim);

        float hitboxDelay = GetAnimationLength(stingAnim) * 0.7f;
        yield return new WaitForSeconds(hitboxDelay);

        DoStingHitbox();

        yield return new WaitForSeconds(GetAnimationLength(stingAnim) * 0.3f);

        yield return new WaitForSeconds(0.3f);

        isAttacking = false;
    }

    void DoStingHitbox()
    {
        if (stingHitboxOrigin == null)
        {
            Debug.LogWarning("ScorpionBoss: stingHitboxOrigin is not assigned!");
            return;
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(stingHitboxOrigin.position, stingHitboxRadius, playerLayer);

        foreach (Collider2D hit in hits)
        {
            PlayerHealth ph = hit.GetComponent<PlayerHealth>();
            if (ph != null) ph.TakeDamage(stingDamage);

            Rigidbody2D prb = hit.GetComponent<Rigidbody2D>();
            if (prb != null)
            {
                float dir = hit.transform.position.x > transform.position.x ? 1f : -1f;
                prb.linearVelocity = Vector2.zero;
                prb.AddForce(new Vector2(dir * launchForceX * 0.6f, launchForceY), ForceMode2D.Impulse);
            }
        }
    }

    //  Contact Damage
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        if (launchTimer < launchCooldown) return; // Already hit recently, ignore repeat contacts

        PlayerHealth ph = collision.gameObject.GetComponent<PlayerHealth>();
        if (ph != null) ph.TakeDamage(damageAmount);

        launchTimer = 0f;
        Rigidbody2D prb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (prb != null)
        {
            float dir = collision.transform.position.x > transform.position.x ? 1f : -1f;
            prb.linearVelocity = Vector2.zero;
            prb.AddForce(new Vector2(dir * launchForceX, launchForceY), ForceMode2D.Impulse);
        }
    }

    // Health / Damage
    public void BossTakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, bossHealth);

        if (flash != null) flash.Flash();

        if (currentHealth <= 0)
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
        float r = 5f;
        float x = Mathf.Clamp(transform.position.x + Random.Range(-r, r), patrolMinX, patrolMaxX);
        currentWanderTarget = new Vector2(x, transform.position.y);
    }

    void FlipSprite(float targetX)
    {
        if (targetX < transform.position.x)
            transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
        else if (targetX > transform.position.x)
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
    }

    Vector3 ClampX(Vector3 pos)
    {
        return new Vector3(Mathf.Clamp(pos.x, patrolMinX, patrolMaxX), pos.y, pos.z);
    }

    float GetAnimationLength(string animName)
    {
        if (animator == null) return 1f;
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == animName) return clip.length;
        }
        Debug.LogWarning($"ScorpionBoss: Could not find clip '{animName}'. Using 1s fallback.");
        return 1f;
    }

    // Gizmos 
    void OnDrawGizmosSelected()
    {
        if (stingHitboxOrigin != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(stingHitboxOrigin.position, stingHitboxRadius);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stingAttackRange);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}