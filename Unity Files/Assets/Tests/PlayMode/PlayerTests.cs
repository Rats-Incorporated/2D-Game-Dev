using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class PlayerTests
{

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator PlayerExistAndMove()
    {
        SceneManager.LoadScene("test-stage");

        float timePassed = 0f;
        while (timePassed < 3f)
        {
            timePassed += Time.deltaTime;
            yield return null;
        }
        timePassed = 0f;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Assert.IsNotNull(player, "Player object must be within the scene");

        var movementIntensity = 10;

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        Assert.IsNotNull(rb, "Player is expected to have Rigidbody2D component");

        var RightDirection = new Vector2(10, 0);
        Vector2 playerPosition = player.transform.position;

        while (timePassed < 3f)
        {
            rb.AddForce(RightDirection * movementIntensity * Time.deltaTime);
            timePassed += Time.deltaTime;
            yield return null;
        }
        timePassed = 0f;
        Assert.Greater(player.transform.position.x, playerPosition.x);

        playerPosition = player.transform.position;

        while (timePassed < 3f)
        {
            rb.AddForce(-RightDirection * movementIntensity * Time.deltaTime);
            timePassed += Time.deltaTime;
            yield return null;
        }
        timePassed = 0f;
        Assert.Greater(playerPosition.x, player.transform.position.x);


    }
}
