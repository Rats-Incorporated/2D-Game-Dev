using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hit : MonoBehaviour
{

    public GameObject boxText;

    void OnTriggerEnter2D(Collider2D collision)
    {
        boxText.SetActive(true);
        Debug.Log("GameObject collided with " + collision.name);
    }
}