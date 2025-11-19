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
    public LogicScript Logic; // game state
    public PlayerGroundJump JumpState; // player jumping & ground collision tracking

    // general movement
    private Rigidbody2D rb; // physics object
    public GameObject cameraTarget; // what the camera is following
    public float movementIntensity = 70.0f; // how fast left/right movements are
    public float downIntensity = 100.0f; // how hard letting go of the spacebar pushes down
    public float jumpVelocity; // jump speed/height
    public float slowSpeed = 50.0f; // how fast the player slows down when pressing neither A or D

    // capacities
    public int jumpTotal = 1;
    public float maxHorizontalSpeed = 12.0f;

    // pickup values (items, etc)
    public bool can_win = false;

    // sprite animation
    Animator animator;
    bool isFacingRight = false;
    float horizontalInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; // Turns off char rotation
        animator = GetComponent<Animator>();
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
            PlayerMove(RightDirection, 1);
        }

        // Move Left
        if (Input.GetKey(KeyCode.A))
        {
            PlayerMove(RightDirection, -1);
        }

        // Not pressing either direction
        if ((!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A)))
        {
            ControlFriction(RightDirection, -1); // moving left
            ControlFriction(RightDirection, 1); // moving right
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

    private void ControlFriction(Vector2 vec, int dir)
    {
        if (rb.linearVelocityX * dir > 0)
        {
            rb.AddForce(-vec * dir * slowSpeed * Time.deltaTime * Mathf.Abs(rb.linearVelocityX));
        }
    }

    // applying the downward force on the player when letting go of space early
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

    // horizontal movement
    private void PlayerMove(Vector2 vec, int dir)
    {
        if (maxHorizontalSpeed > rb.linearVelocityX * dir)
        {
            rb.AddForce(vec * dir * movementIntensity * Time.deltaTime);
        }

        ControlFriction(vec, -dir);
    }

    // handing all the conditions for when the player is jumping
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

