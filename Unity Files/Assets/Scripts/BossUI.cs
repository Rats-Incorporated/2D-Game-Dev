using UnityEngine;
using UnityEngine.UI;

public class BossUI : MonoBehaviour
{
    public GameObject bossHealthBarRoot;
    public Image fillImage;

    private Material runtimeMat;

    public GameObject ExBossText;
    public GameObject BearBossText;

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
    public void ShowBearBossText()
    {
        ExBossText.SetActive(false);
        BearBossText.SetActive(true);

    }
    public void ShowExBossText()
    {
        ExBossText.SetActive(true);
        BearBossText.SetActive(false);

    }
}