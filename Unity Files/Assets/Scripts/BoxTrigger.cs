using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hit : MonoBehaviour
{
    public LogicScript Logic;

    private void Start()
    {
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("Player collided with " + collision.name);

        // Slightly conflicting implementation: Set WinScreen to true here -- Can decide on which implementation to use moving forward.

        if (collision.name != "Rat2")
        {
            Logic.WinGame();
        }

    }
    void OnTriggerExit2D(Collider2D collision)
    {
        // Debug.Log("Player left the collision with " + collision.name);
    }
}