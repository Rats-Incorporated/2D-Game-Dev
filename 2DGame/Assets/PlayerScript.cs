using UnityEngine;
using UnityEngine.InputSystem;

public class playerscript : MonoBehaviour
{
    public Rigidbody2D playerRigidbody;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame

    float speed = 6;
    float jumpspeed = 10;
    bool jumping = false;

    void Update()
    {
        Vector2 velocity = playerRigidbody.linearVelocity;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (!jumping)
            {
                velocity.y = jumpspeed;
                jumping = true;
            }
            
        }

        if (jumping == true)
        {
            if (velocity.y == 0)
            {
                jumping = false;
            }
        }
        

        if (Keyboard.current.leftArrowKey.isPressed)
        {
            velocity.x = -speed;
        }
        else if (Keyboard.current.rightArrowKey.isPressed)
        {
            velocity.x = speed;
        }
        else
        {
            velocity.x = 0;
        }



            playerRigidbody.linearVelocity = velocity;
    }
}
