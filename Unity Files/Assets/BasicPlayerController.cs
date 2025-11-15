using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;
public class BasicPlayerController : MonoBehaviour
{
    public LogicScript Logic;
    // general movement
    private Rigidbody2D rb;
    public GameObject cameraTarget;
    public float movementIntensity;
    public float jumpVelocity;

    // ground collision + jumping
    public int jumpTotal;
    private int jumpCount;
    public float groundCheckDist = 0.51f;
    public float rayWidth = 0.5f;
    public LayerMask groundLayer;
    private bool onGround = false;
    private bool spaceLocked = false;
    private bool alreadyJumped = false;


    // this is to make jumps distinct, basically only rechecking to reset jumps
    // once the player has fully left the ground or this pity timer has expired
    public float pityTimer = 0.2f;
    private float pityTimerStore = 0.0f;

    // this is how long the player has to hold space for max height
    // otherwise, letting go applies a little downward force
    public float jumpTimer = 0.6f;
    private float jumpTimerStore = 0.0f;
    public float downForce = 0.4f;
    private float downTimer = 0.0f;

    // pickup values (items, etc)
    public bool can_win = false;

    Animator animator;
    bool isFacingRight = false;
    float horizontalInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; //Turns off char rotation
        animator = GetComponent<Animator>();

        jumpCount = jumpTotal;
        FlipSpriteLogic();
    }

    void Update()
    {

        // Disable movement if game is paused
        if (Logic.Paused == true)
        {
            return;
        }

        // movement direction vectors
        var UpDirection = new Vector2(0, 10);
        var RightDirection = new Vector2(10, 0);

        // flipping sprite
        horizontalInput = Input.GetAxis("Horizontal");
        FlipSprite();

        // ground collision
        // making some left/right rays to cast based on input distance

        // adjust for player scale size
        float scalex = Mathf.Abs(transform.lossyScale.x);
        float scaley = Mathf.Abs(transform.lossyScale.y);
        float scaleHalf = rayWidth * scalex;
        Vector2 scaleloc = transform.position;

        var left_ray = scaleloc + Vector2.left * scaleHalf;
        left_ray.x -= rayWidth;

        var right_ray = scaleloc + Vector2.right * scaleHalf;
        right_ray.x += rayWidth;

        // create each raycast for checking
        RaycastHit2D center_hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDist * scaley, groundLayer);
        RaycastHit2D left_hit = Physics2D.Raycast(left_ray, Vector2.down, groundCheckDist * scaley, groundLayer);
        RaycastHit2D right_hit = Physics2D.Raycast(right_ray, Vector2.down, groundCheckDist * scaley, groundLayer);

        // if any of the rays hit, set onGround to true
        if (center_hit.collider != null || left_hit.collider != null || right_hit.collider != null)
        {
            onGround = true;
            if (!alreadyJumped)
            {
                jumpCount = jumpTotal; // resets jumps every time ground is touched
            }
        }

        // purely for visual debugging, the visual lines represent the raycast checks
        Debug.DrawRay(transform.position, Vector2.down * groundCheckDist * scaley, center_hit.collider != null ? Color.green : Color.red);
        Debug.DrawRay(left_ray, Vector2.down * groundCheckDist * scaley, left_hit.collider != null ? Color.green : Color.red);
        Debug.DrawRay(right_ray, Vector2.down * groundCheckDist * scaley, right_hit.collider != null ? Color.green : Color.red);

        // Move Up
        if (Input.GetKey(KeyCode.Space))
        {
            // spaceLocked prevents holding the space bar causing all jumps to be used rapidly
            if (jumpCount > 0 && !spaceLocked)
            {
                var curr_vel = rb.linearVelocity;
                if (curr_vel.y < 0)
                {
                    curr_vel.y = 0;
                }
                // removed Time.deltaTime, as this is a set velocity jump, not consistent movement
                rb.linearVelocity = curr_vel + (UpDirection * jumpVelocity);
                spaceLocked = true;
                alreadyJumped = true;
                jumpTimerStore = 0;
                pityTimerStore = 0;
                jumpCount--;
                animator.SetBool("isJumping", true);
            }

            if (spaceLocked)
            {
                jumpTimerStore += Time.deltaTime;
            }

        }

        // prevent jump storage when getting into the air
        if (alreadyJumped)
        {
            // if timer expires, set false
            alreadyJumped = pityTimerIncrement();
            // if timer still hasn't expired, check ground
            // if off ground, set false
            if (alreadyJumped)
            {
                alreadyJumped = onGround;
            }
        }

        // Checking for letting go of Space
        if (Input.GetKeyUp(KeyCode.Space))
        {
            downTimer = jumpTimer - jumpTimerStore;
            spaceLocked = false;
        }

        // adding downward force on letting go of space
        // only applies if traveling upwards
        if (downTimer > 0)
        {
            if (rb.linearVelocity.y > 0)
            {
                downTimer -= Time.deltaTime;
                var downVec = new Vector2(0, downForce * Time.deltaTime);
                rb.linearVelocity = rb.linearVelocity - downVec;
            }
            else
            {
                downTimer = 0;
            }
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

    // Actual Sprite Flip
    void FlipSpriteLogic()
    {
        isFacingRight = !isFacingRight;
        Vector3 ls = transform.localScale;
        ls.x *= -1f;
        transform.localScale = ls;
    }

    //Calls Sprite Flip if conditions are met
    void FlipSprite()
    {
        if (isFacingRight && horizontalInput < 0f || !isFacingRight && horizontalInput > 0f)
        {
            FlipSpriteLogic();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        onGround = true;
        animator.SetBool("isJumping", !onGround);
    }

    // Updates Vars for VSM
    private void FixedUpdate()
    {
        animator.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocity.x));
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
    }
    private bool pityTimerIncrement()
    {
        pityTimerStore += Time.deltaTime;
        if (pityTimerStore <= pityTimer)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // determines if the player can open the exit
    public void pickupkey()
    {
        if (!can_win)
        {
            can_win = true;
        }
    }
}