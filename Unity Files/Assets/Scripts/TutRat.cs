using UnityEngine;

public class TutorialRat : MonoBehaviour
{
    public TutorialStep step;

    private bool triggered = false;

    public void OnHit()
    {
        if (triggered) return;

        if (TutorialManager.Instance.currentStep == step)
        {
            triggered = true;

            TutorialManager.Instance.AdvanceStep();

            gameObject.SetActive(false); // or Destroy(gameObject);
        }
    }
}