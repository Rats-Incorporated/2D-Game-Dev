using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraController : MonoBehaviour
{
    public GameObject player;
    public bool slide = false;

    private PlayerController controller;
    private float max_speed;
    private float max_dist = 2f;
    private float smoothing = 6.7f;

    private void Start()
    {
        controller = player.GetComponent<PlayerController>();
        max_speed = controller.maxHorizontalSpeed;
    }

    void Update()
    {
        Vector3 camPosition = transform.position;
        Vector3 playerPosition = player.transform.position;
        if (!slide)
        {
            camPosition.x = playerPosition.x;
            camPosition.y = playerPosition.y + 1;
            transform.position = camPosition;
        }
        else
        {
            Vector2 playerVec = controller.GetPlayerVector();
            float speed = playerVec.magnitude;

            float x_value = Mathf.Clamp(playerVec.x / max_speed, -max_dist, max_dist);
            float y_value = Mathf.Clamp(playerVec.y / max_speed, -max_dist, max_dist);
            float ratio = Mathf.Clamp01(speed / max_speed);

            float x_target = Mathf.Lerp(playerPosition.x, playerPosition.x + x_value, ratio);
            float y_target = Mathf.Lerp(playerPosition.y, playerPosition.y + y_value, ratio);
            
            float x_pos = Mathf.Lerp(camPosition.x, x_target, Time.deltaTime * smoothing);
            float y_pos = Mathf.Lerp(camPosition.y, y_target, Time.deltaTime * smoothing);
            
            camPosition.x = x_pos;
            camPosition.y = y_pos;
            transform.position = camPosition;
        }


    }
}