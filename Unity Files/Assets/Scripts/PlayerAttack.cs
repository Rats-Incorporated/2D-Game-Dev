using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject hitboxPrefab;
    public float attackDuration = 0.25f;
    public float attackDistance = 2.5f;
    public float attackDmg = 25f;
    public float attackCooldown = 0.7f; // Cooldown between attacks

    private Vector2 facingDirection = Vector2.right; // default facing right
    private bool canAttack = true; // Track if player can attack
    private float cooldownTimer = 0f; // Timer for tracking cooldown

    void Update()
    {
        // Handle attack cooldown timer
        if (!canAttack)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                canAttack = true;
            }
        }

        // Example: detect facing direction from horizontal input
        float horizontal = Input.GetAxisRaw("Horizontal");
        if (horizontal != 0)
            facingDirection = horizontal > 0 ? Vector2.right : Vector2.left;

        // Attack input with cooldown check
        if (Input.GetKeyDown(KeyCode.J) && canAttack)
        {
            SpawnAttack();
        }
    }

    void SpawnAttack()
    {
        // Start cooldown
        canAttack = false;
        cooldownTimer = attackCooldown;

        // Spawn in front of the player based on facing direction
        Vector3 spawnPos = transform.position + (Vector3)(facingDirection * attackDistance);

        // Instantiate hitbox
        GameObject hitbox = Instantiate(hitboxPrefab, spawnPos, Quaternion.identity);

        // Flip the hitbox sprite if facing left
        if (facingDirection == Vector2.left)
        {
            Vector3 scale = hitbox.transform.localScale;
            scale.x *= -1;
            hitbox.transform.localScale = scale;
        }

        // Destroy hitbox after a short time
        Destroy(hitbox, attackDuration);
    }
}
