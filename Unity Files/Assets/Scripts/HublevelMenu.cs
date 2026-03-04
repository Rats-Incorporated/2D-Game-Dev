using UnityEngine;
using UnityEngine.SceneManagement;

public class HublevelMenu : MonoBehaviour
{
    // Update is called once per frame
    private bool playerInExitZone = false;
    private int mainSceneIndex;
    public GameObject canvasGameObject;

    void Start()
    {
        mainSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (canvasGameObject != null)
        {
            canvasGameObject.SetActive(false);
        }
    }


    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Return) && playerInExitZone)
        // {
        //     if (mainSceneIndex == 0)
        //     {
        //         mainSceneIndex = 1;
        //         SceneManager.LoadScene(1); // load sams scene from the main scene
        //     }
        //     else if (mainSceneIndex == 1)
        //     {
        //         mainSceneIndex = 0;
        //         SceneManager.LoadScene(0); // load main scene from sams scene
        //     }
        //     else
        //     {
        //         SceneManager.LoadScene(1); // load sams scene from the main scene
        //     }
        // }
        if (Input.GetKeyDown(KeyCode.Return) && playerInExitZone){
            canvasGameObject.SetActive(!canvasGameObject.activeSelf);
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
    }

     public void Scene1()
    {
        SceneManager.LoadScene(1);
    }
    
     public void Scene2()
    {
        SceneManager.LoadScene(6);
    }
}
