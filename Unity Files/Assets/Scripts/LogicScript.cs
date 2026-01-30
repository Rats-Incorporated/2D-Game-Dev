using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LogicScript : MonoBehaviour
{
    // Game States
    public Text InstructionText;
    public Text BottomRightText;
    public Text WinText;
    public PlayerController player;
    public win_block w_block;
    public GameObject WinScreen;
    public GameObject PauseScreen;
    public GameObject LoseScreen;
    public bool Paused = false;
    private float curTime;
    public Text TimerText;

    void Start()
    {
        curTime = Time.time;
    }

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
        if (!Paused)
        {
            float totaltime = Time.time - curTime;
            TimerText.text = timeString(totaltime);
        }
        if (player.can_win)
        {
            InstructionText.text = "Key Found!";
        }

        if (Input.GetKeyDown(KeyCode.Escape)) // Pause Game
        {
            if (Paused == true)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }


    }

    public void RestartGame()
    {
        Paused = false;
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void WinGame()
    {
        Paused = true;
        Time.timeScale = 0f; // pause physics, animations
        float totaltime = Time.time - curTime;
        WinText.text = "LEVEL COMPLETE\nTime: " + timeString(totaltime);
        TimerText.text = timeString(totaltime);

        string finishTime = timeString(totaltime);


        WinScreen.SetActive(true);
    }

    public void LoseGame()
    {
        Paused = true;
        Time.timeScale = 0f; // pause physics, animations
        LoseScreen.SetActive(true);
    }

    public void Resume()
    {
        Paused = false;
        Time.timeScale = 1.0f; // resume physics, animations
        PauseScreen.SetActive(false);
        WinScreen.SetActive(false);
    }

    public void Pause()
    {
        Paused = true;
        Time.timeScale = 0f;
        PauseScreen.SetActive(true);
    }

    private string timeString(float curT)
    {
        int min = Mathf.FloorToInt(curT / 60);
        int sec = Mathf.FloorToInt(curT % 60);
        int ms = Mathf.FloorToInt(curT * 1000 % 1000) / 10;

        return $"{min:00}:{sec:00}:{ms:00}";

    }
}
