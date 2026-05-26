using UnityEngine;

public class HPPickup : MonoBehaviour
{

    public float healPercent = 100f;

    public AudioClip pickupSound; 

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth player = collision.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.HealDamage(healPercent);
            }

            
            if (pickupSound != null && AudioController.Instance != null)
            {
                AudioController.Instance.PlaySFX(pickupSound);
            }

            gameObject.SetActive(false);
        }
    }

    void Start()
    {

    }

    void Update()
    {

    }
}