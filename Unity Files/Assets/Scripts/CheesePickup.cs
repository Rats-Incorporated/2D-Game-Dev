using UnityEngine;
using System.Collections;

public class CheesePickup : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);





        }
    }


    void Start()
    {

    }

    void Update()
    {

    }
}
