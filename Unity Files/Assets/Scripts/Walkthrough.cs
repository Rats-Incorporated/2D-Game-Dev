using UnityEngine;

public enum TutorialStep
{
    BasicAttack,
    Jump,
    Dash,
    Shuriken,
    Complete
}

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    public TutorialStep currentStep = TutorialStep.BasicAttack;

    [SerializeField] CameraShake cameraShake;

    [Header("Tilemaps")]
    public GameObject platform1;
    public GameObject platform2;
    public GameObject platform3;
    public GameObject finalPlatform;

    [Header("Rats")]
    public GameObject rat2;
    public GameObject rat3;
    public GameObject rat4;
    public GameObject startrat;

    void Awake()
    {
        Instance = this;
    }

    public void AdvanceStep()
    {
        switch (currentStep)
        {
            case TutorialStep.BasicAttack:
                platform1.SetActive(true);
                rat2.SetActive(true);
                currentStep = TutorialStep.Jump;
                startrat.SetActive(false);
                cameraShake.Shake();
                break;

            case TutorialStep.Jump:
                platform2.SetActive(true);
                rat3.SetActive(true);
                currentStep = TutorialStep.Dash;
                cameraShake.Shake();
                break;

            case TutorialStep.Dash:
                platform3.SetActive(true);
                rat4.SetActive(true);
                currentStep = TutorialStep.Shuriken;
                cameraShake.Shake();
                break;

            case TutorialStep.Shuriken:
                platform3.SetActive(false);
                platform2.SetActive(false);
                platform1.SetActive(false);
                rat4.SetActive(false);
                startrat.SetActive(true);
                // possibly could restart and set step back to start?
                cameraShake.Shake();
                break;
        }
    }
}