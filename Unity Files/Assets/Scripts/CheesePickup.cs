using UnityEngine;
using System.Collections;

public class CheesePickup : MonoBehaviour
{

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
