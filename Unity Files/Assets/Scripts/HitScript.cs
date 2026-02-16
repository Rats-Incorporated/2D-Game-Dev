using UnityEngine;
using UnityEngine.SceneManagement;

public class HitScript : MonoBehaviour
{
    public float damageAmount = 25f;
    private float mainSceneIndex;

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
        // for start screen
        if (collision.CompareTag("StartButton"))
        {
            mainSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(0); //should be the first level?
        }
        if (collision.CompareTag("Text"))
        {
            TutorialTrigger trigger = collision.GetComponent<TutorialTrigger>();
            if (trigger != null)
            {
                trigger.Trigger();
            }
        }


        FlyBoss boss = collision.GetComponent<FlyBoss>();
        if (boss != null)
        {
            boss.BossTakeDamage(damageAmount);
        }

        BearBoss bearBoss = collision.GetComponent<BearBoss>();
        if (bearBoss != null)
            bearBoss.BossTakeDamage(damageAmount);


    }
}