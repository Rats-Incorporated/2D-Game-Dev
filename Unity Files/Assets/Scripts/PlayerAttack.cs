using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject hitboxPrefab;
    public float attackDuration = 0.25f;
    public float attackDistance = 2.5f;
    public float attackDmg = 25f;

    private Vector2 facingDirection = Vector2.right; // default facing right

    void Update()
    {
        // Example: detect facing direction from horizontal input
        float horizontal = Input.GetAxisRaw("Horizontal");
        if (horizontal != 0)
            facingDirection = horizontal > 0 ? Vector2.right : Vector2.left;

        // Attack input
        if (Input.GetKeyDown(KeyCode.J))
        {
            SpawnAttack();
        }
    }

    void SpawnAttack()
    {
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