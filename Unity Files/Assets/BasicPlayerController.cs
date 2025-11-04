using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BasicPlayerController : MonoBehaviour
{

    private Rigidbody2D rb;
    public GameObject cameraTarget;
    public float movementIntensity;
    public float jumpVelocity;
    public bool can_win = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        var UpDirection = new Vector2(0, 10);
        var RightDirection = new Vector2(10, 0);

        // Move Up
        if (Input.GetKey(KeyCode.Space))
        {
            var curr_vel = rb.linearVelocity;
            rb.linearVelocity = curr_vel + (UpDirection * jumpVelocity * Time.deltaTime);
        }

        // Move Down
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-UpDirection * movementIntensity * Time.deltaTime);
        }

        // Move Right
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(RightDirection * movementIntensity * Time.deltaTime);
        }

        // Move Left
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(-RightDirection * movementIntensity * Time.deltaTime);
        }
    }
    public void pickupkey()
    {
        if (!can_win)
        {
            can_win = true;
        }
    }
}