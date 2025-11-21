using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_exit : MonoBehaviour
{
    // Update is called once per frame
    private bool playerInExitZone = false;
    private int mainSceneIndex;

    void Start()
    {
        mainSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }
    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && playerInExitZone)
        {
            if (mainSceneIndex == 0){
                mainSceneIndex = 1;
                SceneManager.LoadScene(1); // load sams scene from the main scene
            }
            else if (mainSceneIndex == 1){
                mainSceneIndex = 0;
                SceneManager.LoadScene(0); // load main scene from sams scene
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
}
