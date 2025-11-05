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


    }

    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void WinGame()
    {
        WinScreen.SetActive(true);
    }
    
}
