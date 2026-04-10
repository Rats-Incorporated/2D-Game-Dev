using UnityEngine;
using System.Collections;

public class ShieldPickup : MonoBehaviour
{
    [SerializeField] private GameObject shieldObject;
    [SerializeField] private float dur = 7f;


    void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.CompareTag("Player"))
        {

            shieldObject.SetActive(true);

            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;

            StartCoroutine(ShieldTimer());




        }
    }

    IEnumerator ShieldTimer()
    {

        yield return new WaitForSeconds(dur);
        gameObject.SetActive(false);
        shieldObject.SetActive(false);
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
