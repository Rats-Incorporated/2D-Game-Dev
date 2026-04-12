using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    // other classes
    public LogicScript Logic; // game state
    public PlayerGroundJump JumpState; // player jumping & ground collision tracking
    public PlayerDash DashState; // player dash controls and values
    public PlayerStamina StaminaState; // controls player stamina for movement

    // general movement
    private Rigidbody2D rb; // physics object
    public GameObject cameraTarget; // what the camera is following
    public float movementIntensity = 70.0f; // how fast left/right movements are
    public float downIntensity = 100.0f; // how hard letting go of the spacebar pushes down
    public float jumpVelocity; // jump speed/height
    public float dashVelocity; // dash speed
    public float slowSpeed = 50.0f; // how fast the player slows down when pressing neither A or D

    // capacities
    public int jumpTotal = 1;
    public float maxHorizontalSpeed = 12.0f;

    // pickup values (items, etc)
    public bool can_win = false;

    // sprite animation
    [SerializeField] private Animator animator;
    [SerializeField] private Transform spriteTransform;
    public bool isFacingRight = true;
    float horizontalInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; // Turns off char rotation
        //FlipSpriteLogic();
    }

    void Update()
    {

        // Disable movement if game is paused
        if (Logic.Paused == true)
        {
            return;
        }

        // stamina / dash related vars
        StaminaState.StaminaRegen();
        StaminaState.staminaRegenLock = DashState.inStam;

        // movement direction vectors
        var UpDirection = new Vector2(0, 10);
        var RightDirection = new Vector2(10, 0);

        // flipping sprite
        horizontalInput = Input.GetAxis("LeftRight");
        FlipSprite();

        // reading controller sticks
        Vector2 LeftStick = new Vector2(Input.GetAxis("LeftRight"), Input.GetAxis("UpDown"));
        var StickDeadzone = 0.2f;
        // print(LeftStick);

        // Jumping
        if (Input.GetButtonDown("Jump"))
        {
            PlayerJump(UpDirection);
        }

        // Checking for letting go of Jump
        if (Input.GetButtonUp("Jump"))
        {
            // sets timer to provide downward force
            // longer this timer, more force downward
            JumpState.SetSpaceUpVars();
        }

        // adding downward force on letting go of jump button
        JumpDownforce();

        // Move Down
        if (Input.GetButton("FastFall") || LeftStick.y < -StickDeadzone)
        {
            rb.AddForce(-UpDirection * downIntensity * Time.deltaTime);
        }

        // adding some bools here to help maintain code readability
        bool PlayerGoRight = (Input.GetButton("MoveRight") || LeftStick.x > StickDeadzone);
        bool PlayerGoLeft = (Input.GetButton("MoveLeft") || LeftStick.x < -StickDeadzone);

        // Move Right
        if (PlayerGoRight)
        {
            PlayerMove(RightDirection, 1);
        }

        // Move Left
        if (PlayerGoLeft)
        {
            PlayerMove(RightDirection, -1);
        }

        // Not pressing either direction
        if ((!PlayerGoRight && !PlayerGoLeft) || PlayerGoRight && PlayerGoLeft)
        {
            ControlFriction(RightDirection, -1); // moving left
            ControlFriction(RightDirection, 1); // moving right
        }

        // Adding movement for a dash to the left or the right
        if (PlayerGoRight && Input.GetButtonDown("Dash"))
        {
            PlayerDash(RightDirection, 1);
        }

        if (PlayerGoLeft && Input.GetButtonDown("Dash"))
        {
            PlayerDash(RightDirection, -1);
        }
    }

    // Actual Sprite Flip
    void FlipSpriteLogic()
    {
        isFacingRight = !isFacingRight;
        Vector3 ls = spriteTransform.localScale;
        ls.x *= -1f;
        spriteTransform.localScale = ls;
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
    public void PlayerMove(Vector2 vec, int dir)
    {
        if (maxHorizontalSpeed > rb.linearVelocityX * dir)
        {
            rb.AddForce(vec * dir * movementIntensity * Time.deltaTime);
        }

        ControlFriction(vec, -dir);
    }

    // handing all the conditions for when the player is jumping
    public void PlayerJump(Vector2 UpDirection)
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

    public void PlayerDash(Vector2 vec, int dir)
    {
        if (StaminaState.currentStamina > DashState.stamCost && !DashState.inGCD)
        {
            var curr_vel = rb.linearVelocity;
            // basically if dashing in opposite direction of current momentum
            // kill momentum and pivot
            if (curr_vel.x < 0 && dir > 0)
            {
                curr_vel.x = 0;
            }
            else if (curr_vel.x > 0 && dir < 0)
            {
                curr_vel.x = 0;
            }

            rb.linearVelocity = curr_vel + (vec * dir * dashVelocity);
            DashState.SetDashVars();
        }
    }

    // want the player vector elsewhere so am doing this to make it easier
    public Vector2 GetPlayerVector() { return rb.linearVelocity; }
}

