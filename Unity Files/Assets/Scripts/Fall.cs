using System.Collections;
using UnityEngine.Rendering;
using UnityEngine;

public class fall : MonoBehaviour
{
    private float fallDelay = 1f;
    private float respawnDelay = 6f;

    [SerializeField] private Rigidbody2D rb;

    private Vector3 originalPosition;
    private RigidbodyType2D originalBodyType;

    private void Start()
    {
        originalPosition = transform.position;
        originalBodyType = rb.bodyType;
    }

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


        yield return new WaitForSeconds(respawnDelay);

        // Respawn logic
        rb.bodyType = RigidbodyType2D.Static;
        rb.linearVelocity = Vector2.zero;               // reset leftover falling velocity
        rb.angularVelocity = 0f;

        transform.position = originalPosition;
    }
}
