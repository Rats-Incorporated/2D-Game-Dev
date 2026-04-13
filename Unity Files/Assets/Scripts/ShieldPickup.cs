using UnityEngine;
using System.Collections;

public class ShieldPickup : MonoBehaviour
{
    [SerializeField] private GameObject shieldObject;
    [SerializeField] private float dur = 8f;

    private PlayerInvulnerability invuln;


    void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.CompareTag("Player"))
        {

            invuln = collision.GetComponent<PlayerInvulnerability>();
            invuln.TriggerInvulnerabilityPermaOn();


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
        invuln.TriggerInvulnerabilityPermaOff();
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
