using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TextController : MonoBehaviour
{
    public GameObject player;

    void Update()
    {
        transform.position = player.transform.position;
    }
}