using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraController : MonoBehaviour
{
    public GameObject player;

    void Update()
    {
        Vector3 camPosition = transform.position;
        camPosition.x = player.transform.position.x;
        camPosition.y = player.transform.position.y;
        transform.position = camPosition;
    }
}