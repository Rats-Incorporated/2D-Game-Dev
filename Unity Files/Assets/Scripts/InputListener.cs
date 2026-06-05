using UnityEngine;

public class InputListener : MonoBehaviour
{
    public static InputListener Instance;

    public bool UsingController { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        // Controller axes
        if (Mathf.Abs(Input.GetAxisRaw("LeftRight")) > 0.1f ||
            Mathf.Abs(Input.GetAxisRaw("UpDown")) > 0.1f)
        {
            UsingController = true;
        }

        // Controller buttons
        if (Input.GetKeyDown(KeyCode.JoystickButton0) ||
            Input.GetKeyDown(KeyCode.JoystickButton1) ||
            Input.GetKeyDown(KeyCode.JoystickButton2) ||
            Input.GetKeyDown(KeyCode.JoystickButton3))
        {
            UsingController = true;
        }

        // Keyboard
        if (Input.anyKeyDown)
        {
            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key) &&
                    !key.ToString().StartsWith("Joystick"))
                {
                    UsingController = false;
                    break;
                }
            }
        }
    }
}