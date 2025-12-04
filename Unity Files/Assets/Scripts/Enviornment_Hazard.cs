using Unity.VisualScripting;
using UnityEngine;

public class EnvironmentHazard : MonoBehaviour
{
    public float damageAmount = 25f;
    public float damageInterval = 1.5f;

    private PlayerHealth currentPlayer;
    private float damageTimer;
    private bool playerInHazard = false;

    void Start()
    {
        damageTimer = 0f;
    }

    void Update()
    {
        if (playerInHazard && currentPlayer != null)
        {
            damageTimer += Time.deltaTime;

            if (damageTimer >= damageInterval)
            {
                currentPlayer.TakeDamage(damageAmount);
                damageTimer = 0f;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            currentPlayer = collision.GetComponent<PlayerHealth>();
            if (currentPlayer != null)
            {
                playerInHazard = true;
                currentPlayer.TakeDamage(damageAmount);
                damageTimer = 0f;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInHazard = false;
            currentPlayer = null;
            damageTimer = 0f;
        }
    }
}