using UnityEngine;

public class BossDoor : MonoBehaviour
{

    private GameObject bossRef;
    private bool bossSpawned = false;
    public SpriteRenderer overlayIMG;

    public void SetBoss(GameObject boss)
    {
        bossRef = boss;
        bossSpawned = true;
    }
    
    void Start()
    {
        
    }

    void Update()
    {
        if (bossRef == null && bossSpawned)
        {
            gameObject.SetActive(false);
            overlayIMG.enabled = false;
        }
    }
}
