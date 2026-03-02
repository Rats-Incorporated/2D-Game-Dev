using UnityEngine;

public class CameraSpeedZoom : MonoBehaviour
{
    public PlayerController player;
    public float min_size = 6.5f;
    public float max_size = 9.0f;
    public float smoothing = 5f;
    public float speed_threshold = 3f;
    public float zoom_hold_time = 2f;

    private Camera cam;
    private float max_speed;

    private float holding_ratio;
    private float holding_timer;
    
   void Start()
    {
        cam = GetComponent<Camera>();
        max_speed = player.maxHorizontalSpeed - speed_threshold;
        holding_ratio = 0;
        holding_timer = 0;
    }

    void Update()
    {
        float speed = player.GetPlayerVector().magnitude - speed_threshold;

        // some implementation to basically hold the camera zoom for some time
        // even if the player slows or stops, to allow it to be less disorienting
        if (holding_timer < zoom_hold_time && speed <= 0)
        {
            holding_timer += Time.deltaTime;
        } else if (holding_timer >= zoom_hold_time)
        {
            holding_ratio = 0;
        }
        
        if (speed > 0)
        {
            float ratio = Mathf.Clamp01(speed / max_speed); // clamping between 0 and 1

            if (ratio > holding_ratio)
            {
                holding_ratio = ratio;
                holding_timer = 0;
            }
        }

        float target_size = Mathf.Lerp(min_size, max_size, holding_ratio); // interpolate
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, target_size, Time.deltaTime * smoothing);
    }
}
