using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hit : MonoBehaviour
{

    public SpriteRenderer spriteText;
    public LogicScript Logic;

    private void Start()
    {
        spriteText.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        spriteText.enabled = true;
        Debug.Log("Player collided with " + collision.name);

        // Slightly conflicting implementation: Set WinScreen to true here -- Can decide on which implementation to use moving forward.
        Logic.WinGame();
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        spriteText.enabled = false;
        Debug.Log("Player left the collision with " + collision.name);
    }
}