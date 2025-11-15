using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;

public class fall : MonoBehaviour
{


    private float fallDelay = 1f;
    private float destroyDelay = 2f;


    [SerializeField] private Rigidbody2D rb;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Fall());
        }
    }



    private IEnumerator Fall()
    {

        yield return new WaitForSeconds(fallDelay);
        rb.bodyType = RigidbodyType2D.Dynamic;
        Destroy(gameObject, destroyDelay);

    }

}
