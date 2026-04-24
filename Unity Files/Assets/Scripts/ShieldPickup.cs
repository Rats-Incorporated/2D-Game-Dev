using UnityEngine;
using System.Collections;

public class ShieldPickup : MonoBehaviour
{
    [SerializeField] private GameObject shieldObject;
    [SerializeField] private float dur = 8f;

    private PlayerInvulnerability invuln;

    private float warningDur = 2f;

    Coroutine shieldRoutine;


    void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.CompareTag("Player"))
        {

            invuln = collision.GetComponent<PlayerInvulnerability>();
            invuln.TriggerInvulnerabilityPermaOn();


            shieldObject.SetActive(true);
            Material shieldMat = shieldObject.GetComponent<Renderer>().material;
            shieldMat.SetFloat("_Pulse", 1f);

            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;


            if (shieldRoutine != null)
            {
                StopCoroutine(shieldRoutine);
            }


            shieldRoutine = StartCoroutine(ShieldTimer());




        }
    }

    IEnumerator ShieldTimer()
    {
        yield return new WaitForSeconds(dur - warningDur);

        Material shieldMat = shieldObject.GetComponent<Renderer>().material;

        float warningTime = warningDur;

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

        shieldMat.SetFloat("_Pulse", 1f);
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
