using UnityEngine;
using System.Collections;

public class ShieldPickup : MonoBehaviour
{
    [SerializeField] private GameObject shieldObject;
    [SerializeField] private float dur = 8f;

    private PlayerInvulnerability invuln;

    private float warningTime = 2f;


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
        yield return new WaitForSeconds(dur-warningTime);

        Material shieldMat = shieldObject.GetComponent<Renderer>().material;


        while (warningTime > 0f)
        {
            warningTime -= Time.deltaTime;

            float pulse = Mathf.PingPong(Time.time * 5f, 1f);
            shieldMat.SetFloat("_Pulse", pulse);

            yield return null; 
        }

        shieldMat.SetFloat("_Pulse", 0f);

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
