using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LogicScript : MonoBehaviour
{
    public Text InstructionText;
    public Text BottomRightText;
    public BasicPlayerController player;
    public win_block w_block;
    public GameObject WinScreen;
    public GameObject PauseScreen;
    public bool Paused = false;

    public void TempMessage(string msg, float dur = 2f)
    {
        BottomRightText.text = msg;

        CancelInvoke(nameof(ClearMessage));
        Invoke(nameof(ClearMessage), dur);
    }

    private void ClearMessage()
    {
        BottomRightText.text = "";
    }
    private void Update()
    {
        if (player.can_win)
        {
            InstructionText.text = "Key Found!";
        }

        if (Input.GetKey(KeyCode.Escape)) // Pause Game
        {
            PauseScreen.SetActive(true);
            Time.timeScale = 0f; // pause physics and animations
        }


    }

    public void restartGame()
    {
        Paused = false;
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void WinGame()
    {
        Paused = true;
        Time.timeScale = 0f; // pause physics, animations
        WinScreen.SetActive(true);
    }

    public void Resume()
    {
        Paused = false;
        Time.timeScale = 1.0f; // resume physics, animations
        PauseScreen.SetActive(false);
    }

}
