using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HublevelMenu : MonoBehaviour
{
    // Update is called once per frame
    private bool playerInExitZone = false;
    private int mainSceneIndex;
    public GameObject canvasGameObject;
    public GameObject loadingScreen;
    public GameObject uicanvas;
    public bool Paused = false;
    public float sceneDelay = 3f;

    void Start()
    {
        mainSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (canvasGameObject != null)
        {
            canvasGameObject.SetActive(false);
        }
    }

    public void Resume()
    {
        Paused = false;
        Time.timeScale = 1.0f; // resume physics, animations
        canvasGameObject.SetActive(false);

    }

    public void Pause()
    {
        Paused = true;
        Time.timeScale = 0f;
        canvasGameObject.SetActive(true);
    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Return) && playerInExitZone)
        {
            canvasGameObject.SetActive(!canvasGameObject.activeSelf);
            if (Paused == false)
            {
                Pause();
            }
            else
            {
                Resume();
            }


        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInExitZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInExitZone = false;
        }
    }

    public void ExitMenu()
    {
        canvasGameObject.SetActive(!canvasGameObject.activeSelf);
        Resume();
    }

    public void Scene1()
    {
        Resume();
        StartCoroutine(LoadSceneAfterDelay("CutScene3")); // Example: delay of 4 seconds
    }

    public void Scene2()
    {
        Resume();
        StartCoroutine(LoadSceneAfterDelay("CutScene1")); // Example: delay of 4 seconds
    }

    public void Scene3()
    {
        Resume();
        StartCoroutine(LoadSceneAfterDelay("CutScene3")); // Example: delay of 4 seconds
    }


    IEnumerator LoadSceneAfterDelay(string sceneName)
    {
        Resume();

        // play animation here if needed
        // trainAnimator.SetTrigger("Depart");

        yield return new WaitForSeconds(2f);
        loadingScreen.SetActive(true);
        uicanvas.SetActive(false);
        yield return new WaitForSeconds(4f);


        SceneManager.LoadScene(sceneName);
    }
}
