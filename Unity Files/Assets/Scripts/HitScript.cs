using UnityEngine;

public class HitScript : MonoBehaviour
{
    public float damageAmount = 25f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyDefault enemy = collision.GetComponent<EnemyDefault>();
            if (enemy != null)
            {
                enemy.EnemyTakeDamage(damageAmount);
            }
        }

       
        FlyBoss boss = collision.GetComponent<FlyBoss>();
        if (boss != null)
        {
            boss.BossTakeDamage(damageAmount);
        }



    }
}