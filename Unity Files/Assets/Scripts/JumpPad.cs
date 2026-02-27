using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float bounce = 20f;
    public float cooldown = 0.4f;
    private float cd_timer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && cd_timer >= cooldown)
        {
            cd_timer = 0;
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb.linearVelocityY < 0)
            {
                rb.linearVelocityY = 0f;
            }
            rb.AddForce(Vector2.up * bounce, ForceMode2D.Impulse);
        }
    }

    private void Start()
    {
        cd_timer = cooldown;
    }

    private void Update()
    {
        if (cd_timer <= cooldown)
        {
            cd_timer += Time.deltaTime;
        }
    }
}
