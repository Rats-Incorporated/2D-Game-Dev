using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class PlayerTests
{

    
    [UnityTest]
    public IEnumerator PlayerExistAndMove()
    {
        // this test loads our main scene and checks for:
        // player existing, player left/right movement working
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

        var movementIntensity = 4;

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        Assert.IsNotNull(rb, "Player is expected to have Rigidbody2D component");

        var RightDirection = new Vector2(10, 0);
        Vector2 playerPosition = player.transform.position;

        rb.AddForce(RightDirection * movementIntensity);
        while (timePassed < 3f)
        {
            timePassed += Time.deltaTime;
            yield return null;
        }
        timePassed = 0f;
        Assert.Greater(player.transform.position.x, playerPosition.x);

        playerPosition = player.transform.position;

        rb.AddForce(-RightDirection * movementIntensity);
        while (timePassed < 3f)
        {
            timePassed += Time.deltaTime;
            yield return null;
        }
        timePassed = 0f;
        Assert.Greater(playerPosition.x, player.transform.position.x);


    }
}
