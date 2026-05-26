using UnityEngine;
using System.Collections;


public class CheesePickup : MonoBehaviour
{
    public AudioClip pickupcheese;
    bool preventDuplicate = false;

    void OnTriggerEnter2D(Collider2D collision)
    {

        if (preventDuplicate == true)
        {
            return;
        }
        if (collision.CompareTag("Player"))
        {

            preventDuplicate = true;

            Destroy(gameObject);



            //collectable count
            PlayerController pc = collision.GetComponentInParent<PlayerController>();

            if (pickupcheese != null && AudioController.Instance != null)
            {
                AudioController.Instance.PlaySFX(pickupcheese);
            }


            if (pc != null)
            {
                pc.Logic.AddCollectableCount();
            }





        }
    }


    void Start()
    {

    }

    void Update()
    {

    }
}
