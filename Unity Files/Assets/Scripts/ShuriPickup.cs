using UnityEngine;

public class ShuriPickup : MonoBehaviour
{
    public AudioClip pickupSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {

            //preventDuplicate = true;

            if (AudioController.Instance != null)
            {
                AudioController.Instance.PlaySFX(pickupSound);
            }

            Destroy(gameObject);

            PlayerShuriken player = collision.GetComponentInParent<PlayerShuriken>();

            player.shuriCount = 3;






        }
    }

}