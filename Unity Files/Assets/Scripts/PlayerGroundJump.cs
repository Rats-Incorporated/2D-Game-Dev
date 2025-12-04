using UnityEngine;

public class PlayerGroundJump : MonoBehaviour
{
    // other classes
    public PlayerController player;

    // modifiable vars
    public float groundCheckDist = 0.51f;
    public float rayWidth = 0.25f;
    public LayerMask groundLayer;

    public float pityTimer = 0.2f;
    public float jumpTimer = 0.6f;
    public float downForce = 0.4f;

    // sharing vars
    public bool onGround = false;
    public bool spaceLocked = false;
    public bool alreadyJumped = false;
    public bool downForceActive = false;

    // private vars
    private int jumpTotal;
    private int jumpCount;
    private float pityTimerStore = 0.0f;
    private float jumpTimerStore = 0.0f;
    private float downTimer = 0.0f;

    void Start()
    {
        jumpTotal = player.jumpTotal;
        jumpCount = jumpTotal;
    }

    void Update()
    {
        GetGround();
        LeaveGroundCheck();

        if (spaceLocked)
        {
            JumpTimerIncrement();
        }

        CheckDownForceActive();
    }

    // ground collision
    // this is checking purely for if/when to reset jumps (jumpCount) AND if the player is on the ground (onGround)
    private void GetGround()
    {
        // making some left/right rays to cast based on the given distance
        // adjust for player scale size
        float scalex = Mathf.Abs(player.transform.lossyScale.x);
        float scaley = Mathf.Abs(player.transform.lossyScale.y);
        float scaleHalf = rayWidth * scalex;
        Vector2 scaleloc = player.transform.position;

        var left_ray = scaleloc + Vector2.left * scaleHalf;
        left_ray.x -= rayWidth;

        var right_ray = scaleloc + Vector2.right * scaleHalf;
        right_ray.x += rayWidth;

        // create each raycast for checking
        RaycastHit2D center_hit = Physics2D.Raycast(player.transform.position, Vector2.down, groundCheckDist * scaley, groundLayer);
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
        else
        {
            onGround = false;
        }

        // purely for visual debugging, the visual lines represent the raycast checks
        Debug.DrawRay(player.transform.position, Vector2.down * groundCheckDist * scaley, center_hit.collider != null ? Color.green : Color.red);
        Debug.DrawRay(left_ray, Vector2.down * groundCheckDist * scaley, left_hit.collider != null ? Color.green : Color.red);
        Debug.DrawRay(right_ray, Vector2.down * groundCheckDist * scaley, right_hit.collider != null ? Color.green : Color.red);
    }

    // prevent jump storage when getting into the air
    // this is checking for if the player has left the ground OR pity timer expired (alreadyJumped)
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

    // function to check for when the downforce factor is active
    private void CheckDownForceActive()
    {
        if (downTimer > 0.0f)
        {
            downTimer -= Time.deltaTime;
            downForceActive = true;
        }
        else
        {
            downForceActive = false;
            downTimer = 0.0f;
        }
    }

    // function to help with pity timer storage (pityTimerStore)
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

    // function to increment the jumpTimerStore
    private void JumpTimerIncrement()
    {
        jumpTimerStore += Time.deltaTime;
    }

    public int GetJumpCount()
    {
        return jumpCount;
    }

    // on jump vars
    public void SetJumpVars()
    {
        jumpTimerStore = 0;
        pityTimerStore = 0;
        jumpCount--;
        spaceLocked = true;
        alreadyJumped = true;
    }

    // on letting go of space vars
    public void SetSpaceUpVars()
    {
        downTimer = jumpTimer - jumpTimerStore;
        spaceLocked = false;
    }
}
