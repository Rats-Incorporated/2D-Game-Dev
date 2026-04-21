using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Hitbox")]
    public GameObject hitboxPrefab;
    public float attackDistance = 2.5f;
    public float attackDuration = 0.25f;
    public float attackDmg = 25f;

    [Header("Cooldown")]
    public float attackCooldown = 0.1f;
    private bool canAttack = true;
    private float cooldownTimer = 0f;

    [Header("References")]
    [SerializeField] private Animator anim;
    public PlayerController playerController; // assign in inspector

    //private bool primaryAttack = true;

    void Update()
    {
        // Handle cooldown
        if (!canAttack)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
                canAttack = true;
        }

        // Determine facing direction using PlayerController
        Vector2 facingDirection = playerController.isFacingRight ? Vector2.right : Vector2.left;

        // Attack input
        if (Input.GetButton("Attack") && canAttack)
        {
            SpawnAttack(facingDirection);

            int primaryAttack = Random.Range(0, 2);


            if (primaryAttack==0)
            {
                anim.SetTrigger("PlayerAttack2");
            }
            else
            {
                anim.SetTrigger("PlayerAttack3");
            }
           // primaryAttack = !primaryAttack;
            
        }
        if (Input.GetButton("Attack2") && canAttack)
        {
            SpawnAttack(facingDirection);
            anim.SetTrigger("PlayerAttack");
        }
        if (Input.GetButton("AttackFlurry") && canAttack)
        {
            SpawnAttack(facingDirection);
            anim.SetTrigger("PlayerAttackFlurry");
        }
    }

    void SpawnAttack(Vector2 facingDirection)
    {
        // Start cooldown
        canAttack = false;
        cooldownTimer = attackCooldown;

        // Spawn position relative to player
        Vector3 localOffset = new Vector3(facingDirection.x * attackDistance, 0f, 0f);
        Vector3 spawnPos = transform.position + localOffset;

        // Instantiate hitbox
        GameObject hitbox = Instantiate(hitboxPrefab, spawnPos, Quaternion.identity);

        // Parent to player so it moves with them
        hitbox.transform.SetParent(transform);

        // Flip hitbox sprite if facing left
        if (!playerController.isFacingRight)
        {
            Vector3 scale = hitbox.transform.localScale;
            scale.x *= -1f;
            hitbox.transform.localScale = scale;
        }

        // Optional: give hitbox Rigidbody2D horizontal velocity to match player movement
        Rigidbody2D rb = hitbox.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 playerVel = playerController.GetPlayerVector();
            rb.linearVelocity = new Vector2(playerVel.x, 0); // only horizontal
        }

        // Destroy hitbox after attack duration
        Destroy(hitbox, attackDuration);
    }
}