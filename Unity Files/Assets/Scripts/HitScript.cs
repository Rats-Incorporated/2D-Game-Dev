using UnityEngine;
using UnityEngine.SceneManagement;

public class HitScript : MonoBehaviour
{
    public float damageAmount = 25f;
    private float mainSceneIndex;

    [Header("Downward Attack / Pogo")]
    public bool isDownwardAttack = false;
    public PlayerController player;
    public float pogoStrength = 14f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // =========================
        // ENEMIES
        // =========================

        if (collision.CompareTag("Enemy"))
        {
            EnemySlap slapEnemy = collision.GetComponentInParent<EnemySlap>();
            if (slapEnemy != null)
            {
                slapEnemy.EnemyTakeDamage(damageAmount);
                ApplyPogo();
            }

            EnemyDefault defaultEnemy = collision.GetComponentInParent<EnemyDefault>();
            if (defaultEnemy != null)
            {
                defaultEnemy.EnemyTakeDamage(damageAmount);
                ApplyPogo();
            }
        }

        // =========================
        // START BUTTON
        // =========================

        if (collision.CompareTag("StartButton"))
        {
            mainSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(1);
        }

        // =========================
        // TUTORIAL TEXT
        // =========================

        if (collision.CompareTag("Text"))
        {
            TutorialTrigger trigger = collision.GetComponent<TutorialTrigger>();

            if (trigger != null)
            {
                trigger.Trigger();
            }
        }

        // =========================
        // TUTORIAL RAT
        // =========================

        TutorialRat rat = collision.GetComponent<TutorialRat>();

        if (rat != null)
        {
            rat.OnHit();
            ApplyPogo();
        }

        // =========================
        // BOSSES
        // =========================

        FlyBoss boss = collision.GetComponent<FlyBoss>();

        if (boss != null)
        {
            boss.BossTakeDamage(damageAmount);
            ApplyPogo();
        }

        BearBoss bearBoss = collision.GetComponent<BearBoss>();

        if (bearBoss != null)
        {
            bearBoss.BossTakeDamage(damageAmount);
            ApplyPogo();
        }

        ScorpionBoss scorpBoss = collision.GetComponentInParent<ScorpionBoss>();
        if (scorpBoss != null)
        {
            scorpBoss.BossTakeDamage(damageAmount);
            ApplyPogo();
        }


    }

    void ApplyPogo()
    {
        // only apply pogo if this hitbox was spawned as a downward attack
        if (isDownwardAttack && player != null)
        {
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                // only pogo while falling downward
                if (rb.linearVelocity.y <= 0)
                {
                    Vector2 vel = rb.linearVelocity;
                    vel.y = pogoStrength;
                    rb.linearVelocity = vel;
                }
            }
        }
    }
}