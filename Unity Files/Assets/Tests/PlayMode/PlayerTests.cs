using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class PlayerTests
{

    [UnityTest]
    public IEnumerator PlayerExist()
    {
        // this test loads the test scene and makes sure the player exists
        SceneManager.LoadScene("test-base");

        float timePassed = 0f;
        while (timePassed < 0.3f)
        {
            timePassed += Time.deltaTime;
            yield return null;
        }
        timePassed = 0f;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Assert.IsNotNull(player, "Player object must be within the scene");

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        Assert.IsNotNull(rb, "Player is expected to have Rigidbody2D component");

        PlayerController pc = player.GetComponent<PlayerController>();
        Assert.IsNotNull(pc, "Player is expected to have a PlayerController component");

    }

    [UnityTest]
    public IEnumerator PlayerMove()
    {
        SceneManager.LoadScene("test-base");

        float timePassed = 0f;
        while (timePassed < 1.2f)
        {
            timePassed += Time.deltaTime;
            yield return null;
        }
        timePassed = 0f;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerController pc = player.GetComponent<PlayerController>();

        float starterPlayerX = player.transform.position.x;
        while (timePassed < 1f)
        {
            pc.PlayerMove(new Vector2(10, 0), -1);
            timePassed += Time.deltaTime;
            yield return null;
        }
        timePassed = 0f;
        float endPlayerX = player.transform.position.x;
        Assert.Greater(starterPlayerX, endPlayerX);

        starterPlayerX = player.transform.position.x;
        while (timePassed < 1f)
        {
            pc.PlayerMove(new Vector2(10, 0), 1);
            timePassed += Time.deltaTime;
            yield return null;
        }
        endPlayerX = player.transform.position.x;
        Assert.Less(starterPlayerX, endPlayerX);
    }

    [UnityTest]
    public IEnumerator PlayerJump()
    {
        SceneManager.LoadScene("test-base");

        float timePassed = 0f;
        while (timePassed < 2f)
        {
            timePassed += Time.deltaTime;
            yield return null;
        }
        timePassed = 0f;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerController pc = player.GetComponent<PlayerController>();

        float starterPlayerY = player.transform.position.y; 
        while (timePassed < 1f)
        {
            pc.PlayerJump(new Vector2(0, 10));
            timePassed += Time.deltaTime;
            yield return null;
        }
        float endPlayerY = player.transform.position.y;
        Assert.Less(starterPlayerY, endPlayerY);
    }
}
