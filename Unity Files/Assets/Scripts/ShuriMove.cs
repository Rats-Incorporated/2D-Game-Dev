using UnityEngine;

public class Shuriken : MonoBehaviour
{
    public float speed = 15f;
    public float damage = 20f;
    public float lifetime = 5f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // New Initialize method that takes player velocity
    public void Initialize(Vector2 direction, Vector2 playerVelocity)
    {
        // Only add the horizontal component of the player velocity
        Vector2 inheritedVelocity = new Vector2(playerVelocity.x, 0);

        rb.linearVelocity = direction.normalized * speed + inheritedVelocity;

        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TutorialRat rat = collision.GetComponentInParent<TutorialRat>();
        if (rat != null)
        {
            rat.OnHit();
            Destroy(gameObject);
            return;
        }

        // Damage enemy
        FlyBoss boss = collision.GetComponent<FlyBoss>();
        if (boss != null)
        {
            boss.BossTakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        BearBoss bearboss = collision.GetComponent<BearBoss>();
        if (bearboss != null)
        {
            bearboss.BossTakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        if (collision.CompareTag("Enemy"))
        {
            EnemySlap slapEnemy = collision.GetComponentInParent<EnemySlap>();
            if (slapEnemy != null)
            {
                slapEnemy.EnemyTakeDamage(damage);
            }

            EnemyDefault defaultEnemy = collision.GetComponentInParent<EnemyDefault>();
            if (defaultEnemy != null)
            {
                defaultEnemy.EnemyTakeDamage(damage);
            }
        }



        // Destroy on ANY non-player object
        if (!collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}