using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering.UI;
using static UnityEngine.LightAnchor;
using static UnityEngine.RuleTile.TilingRuleOutput;
public class PlayerController : MonoBehaviour
{
    // other classes
    public LogicScript Logic;
    public PlayerGroundJump JumpState;

    // general movement
    private Rigidbody2D rb;
    public GameObject cameraTarget;
    public float movementIntensity;
    public float jumpVelocity;

    // ground collision + jumping
    public int jumpTotal;
    private int jumpCount;
    public float groundCheckDist = 0.51f;
    public float rayWidth = 0.25f;
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
        rb.freezeRotation = true; // Turns off char rotation
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

        // ground collision call
        GetGround();

        // Jumping
        if (Input.GetKey(KeyCode.Space))
        {
            PlayerJump(UpDirection);
        }

        // Making sure the player leaves the ground or waits before looking to reset jumps
        LeaveGroundCheck();

        // Checking for letting go of Space
        if (Input.GetKeyUp(KeyCode.Space))
        {
            // sets timer to provide downward force
            // longer this timer, more force downward
            downTimer = jumpTimer - jumpTimerStore;
            spaceLocked = false;
        }


        // adding downward force on letting go of space
        JumpDownforce();

        // Move Down
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-UpDirection * 100 * Time.deltaTime);
        }

        // Move Right
        if (Input.GetKey(KeyCode.D))
        {
            rb.linearVelocityX = movementIntensity;
            // rb.AddForce(RightDirection * movementIntensity * Time.deltaTime);
        }

        // Move Left
        if (Input.GetKey(KeyCode.A))
        {
            rb.linearVelocityX = -movementIntensity;
            // rb.AddForce(-RightDirection * movementIntensity * Time.deltaTime);
        }

        // Not pressing either direction
        // Right now this sets to zero, but could implement a tiny function that does a little slide into velocity of 0
        // so that it gives the impression of a little momentum without being uncontrollable
        // would also be worth looking into code of just switching standing position if clicking the opposite the last direction without moving
        if ((!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A)))
        {
            rb.linearVelocityX = 0;
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

    // Calls Sprite Flip if conditions are met
    void FlipSprite()
    {
        if (isFacingRight && horizontalInput < 0f || !isFacingRight && horizontalInput > 0f)
        {
            FlipSpriteLogic();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Unsure of how this might interact with other functions so disabling for now
        // onGround = true;
        animator.SetBool("isJumping", false);
    }

    // Updates Vars for VSM
    private void FixedUpdate()
    {
        animator.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocity.x));
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
    }
    private bool PityTimerIncrement()
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
    public void PickupKey()
    {
        if (!can_win)
        {
            can_win = true;
        }
    }

    // ground collision
    private void GetGround()
    {
        // making some left/right rays to cast based on the given distance
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
    }

    // Jump Calculation
    private void PlayerJump(Vector2 UpDirection)
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
    private void LeaveGroundCheck()
    {
        if (alreadyJumped)
        {
            // if timer expires, set false
            alreadyJumped = PityTimerIncrement();
            // if timer still hasn't expired, check ground
            // if off ground, set false
            if (alreadyJumped)
            {
                alreadyJumped = onGround;
            }
        }
    }

    // Provides a downward force when letting go of space early
    private void JumpDownforce()
    {
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
    }
}

