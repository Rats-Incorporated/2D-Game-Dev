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

    public void Initialize(Vector2 direction)
    {
        rb.linearVelocity = direction.normalized * speed;
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Damage enemy

        FlyBoss boss = collision.GetComponent<FlyBoss>();
        if (boss != null)
        {
            boss.BossTakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        EnemyDefault enemy = collision.GetComponentInParent<EnemyDefault>();
        if (enemy != null)
        {
            enemy.EnemyTakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        // Destroy on ANY non-player object
        if (!collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}