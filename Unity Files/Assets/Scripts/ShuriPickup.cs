using UnityEngine;

public class ShuriPickup : MonoBehaviour
{
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

            Destroy(gameObject);

            PlayerShuriken player = collision.GetComponentInParent<PlayerShuriken>();

            player.shuriCount = 3;








        }
    }

}
