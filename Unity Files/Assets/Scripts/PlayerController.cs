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
    public float downIntensity = 100.0f;
    public float jumpVelocity;

    // ground collision + jumping
    public int jumpTotal;
    //private int jumpCount;
    //public float groundCheckDist = 0.51f;
    //public float rayWidth = 0.25f;
    //public LayerMask groundLayer;
    //private bool onGround = false;
    //private bool spaceLocked = false;
    //private bool alreadyJumped = false;

    // this is to make jumps distinct, basically only rechecking to reset jumps
    // once the player has fully left the ground or this pity timer has expired
    //public float pityTimer = 0.2f;
    //private float pityTimerStore = 0.0f;

    // this is how long the player has to hold space for max height
    // otherwise, letting go applies a little downward force
    //public float jumpTimer = 0.6f;
    //private float jumpTimerStore = 0.0f;
    //public float downForce = 0.4f;
    //private float downTimer = 0.0f;

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

        //jumpCount = jumpTotal;
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


        // Jumping
        if (Input.GetKey(KeyCode.Space))
        {
            PlayerJump(UpDirection);
        }

        // Checking for letting go of Space
        if (Input.GetKeyUp(KeyCode.Space))
        {
            // sets timer to provide downward force
            // longer this timer, more force downward
            JumpState.SetSpaceUpVars();
        }


        // adding downward force on letting go of space
        JumpDownforce();

        // Move Down
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-UpDirection * downIntensity * Time.deltaTime);
        }

        // Move Right
        if (Input.GetKey(KeyCode.D))
        {
            //rb.linearVelocityX = movementIntensity;
            rb.AddForce(RightDirection * movementIntensity * Time.deltaTime);
        }

        // Move Left
        if (Input.GetKey(KeyCode.A))
        {
            //rb.linearVelocityX = -movementIntensity;
            rb.AddForce(-RightDirection * movementIntensity * Time.deltaTime);
        }

        // Not pressing either direction
        // Right now this sets to zero, but could implement a tiny function that does a little slide into velocity of 0
        // so that it gives the impression of a little momentum without being uncontrollable
        // would also be worth looking into code of just switching standing position if clicking the opposite the last direction without moving
        //if ((!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A)))
        //{
        //    rb.linearVelocityX = 0;
        //}
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

    // determines if the player can open the exit
    public void PickupKey()
    {
        if (!can_win)
        {
            can_win = true;
        }
    }

    private void JumpDownforce()
    {
        if (JumpState.downForceActive == true)
        {
            if (rb.linearVelocity.y > 0)
            {
                var downVec = new Vector2(0, JumpState.downForce * Time.deltaTime);
                rb.linearVelocity = rb.linearVelocity - downVec;
            }
        }
    }

    private void PlayerJump (Vector2 UpDirection)
    {
        // spaceLocked prevents holding the space bar causing all jumps to be used rapidly
        if (JumpState.GetJumpCount() > 0 && !JumpState.spaceLocked)
        {
            var curr_vel = rb.linearVelocity;
            if (curr_vel.y < 0)
            {
                curr_vel.y = 0;
            }
            // removed Time.deltaTime, as this is a set velocity jump, not consistent movement
            rb.linearVelocity = curr_vel + (UpDirection * jumpVelocity);
            animator.SetBool("isJumping", true);
            JumpState.SetJumpVars();
        }
    }
}

