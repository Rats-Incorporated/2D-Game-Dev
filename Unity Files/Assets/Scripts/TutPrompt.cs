using TMPro;
using UnityEngine;

public class TutorialPrompt : MonoBehaviour
{
    public TMP_Text promptText;

    [TextArea]
    public string keyboardPrompt;

    [TextArea]
    public string controllerPrompt;

    void Update()
    {
        if (InputListener.Instance != null &&
            InputListener.Instance.UsingController)
        {
            promptText.text = controllerPrompt;
        }
        else
        {
            promptText.text = keyboardPrompt;
        }
    }
}