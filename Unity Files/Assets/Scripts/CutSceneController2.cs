using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutsceneController2 : MonoBehaviour
{
    public Transform[] shots;
    public string[] shotNames;

    public Camera mainCamera;
    public Text locationText;

    public float shotDuration = 2.5f;

    private string gameplaySceneName = "Desert";

    IEnumerator Start()
    {
        for (int i = 0; i < shots.Length; i++)
        {
            // Move camera instantly to each shot
            mainCamera.transform.position = new Vector3(
                shots[i].position.x,
                shots[i].position.y,
                mainCamera.transform.position.z
            );

            // Update label
            locationText.text = shotNames[i];

            // Wait at this shot
            yield return new WaitForSeconds(shotDuration);
        }

        // Small pause before transition (optional but nice)
        yield return new WaitForSeconds(1.5f);

        // Load gameplay scene
        SceneManager.LoadScene(gameplaySceneName);
    }
}