using UnityEngine;
using UnityEngine.UI;

public class BossUI : MonoBehaviour
{
    public GameObject bossHealthBarRoot;
    public Image fillImage;

    private Material runtimeMat;

    public GameObject ExBossText;
    public GameObject BearBossText;
    public GameObject FlyBossText;
    public GameObject ScorpionBossText;

    public void SetFill(float value)
    {
        if (runtimeMat != null)
            runtimeMat.SetFloat("_Fill", value);
    }

    public void Init(Material mat)
    {
        runtimeMat = Instantiate(mat);
        fillImage.material = runtimeMat;
    }

    public void Hide()
    {
        bossHealthBarRoot.SetActive(false);
    }
    public void Show()
    {
        Debug.Log("Entered Cave Teleporter");
        bossHealthBarRoot.SetActive(true);
        SetFill(1);


    }

    public void HideAllNames()
    {
        ExBossText.SetActive(false);
        BearBossText.SetActive(false);
        FlyBossText.SetActive(false);
        ScorpionBossText.SetActive(false);
    }
    public void ShowBearBossText()
    {
        HideAllNames();
        BearBossText.SetActive(true);

    }
    public void ShowExBossText()
    {
        HideAllNames();
        ExBossText.SetActive(true);

    }

    public void ShowFlyBossText()
    {
        HideAllNames();
        FlyBossText.SetActive(true);
    }

    public void ShowScorpionBossText()
    {
        HideAllNames();
        ScorpionBossText.SetActive(true);
    }
}