using UnityEngine;
using UnityEngine.UI;

public class BossUI : MonoBehaviour
{
    public GameObject bossHealthBarRoot;
    public Image fillImage;

    private Material runtimeMat;

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
}