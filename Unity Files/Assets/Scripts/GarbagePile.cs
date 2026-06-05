using UnityEngine;
using UnityEngine.UIElements;

public class garbage_pile : MonoBehaviour
{
    public float eatTimeRequired = 5f;   // total time needed to eat
    private float currentEatTime = 0f;   // current eat time
    private bool playerInRange = false;  // is player inside trigger?


    public GameObject bossPrefab;        // boss to spawn
    public Transform bossSpawnPoint;     // where boss appears
    private bool bossSpawned = false;    // prevent double spawn

    private GameObject bossRef;          // tracking the spawned boss
    public BossDoor bossDoor;            // door object


    public GameObject GarbageMessageContainer;
    public GameObject GarbageLoadingBarContainer;
    public GameObject GarbageLoadingBar;
    private CanvasGroup cg;

    public Material bossHealthBarMaterial;
    public GameObject bossHealthBar;

    public BossUI BossUI;


    public AudioClip eatingSound;

    private bool eatingSoundPlayed = false;

    void Start()
    {
        cg = GarbageMessageContainer.GetComponent<CanvasGroup>();
        cg.alpha = 0;
    }
    void Update()
    {
        if (playerInRange && !bossSpawned)
        {
            if (Input.GetButton("Interact"))
            {

                if (!eatingSoundPlayed)
                {
                    eatingSoundPlayed = true;

                    if (AudioController.Instance != null)
                    {
                        AudioController.Instance.PlaySFX(eatingSound);
                    }
                }


                currentEatTime += Time.deltaTime;

                Debug.Log("Eating progress: " + currentEatTime + " / " + eatTimeRequired);

                if (currentEatTime > 0)
                {
                    GarbageLoadingBarContainer.SetActive(true);
                    float eatTimePercent = currentEatTime / eatTimeRequired;
                    GarbageLoadingBar.GetComponent<RectTransform>().localScale = new Vector3(eatTimePercent * 1.0f, 1f, 1f);
                }

                if (currentEatTime >= eatTimeRequired)
                {
                    SpawnBoss();
                    Destroy(gameObject);
                }
            }

            else
            {
                eatingSoundPlayed = false;
            }



        }
    }

    void SpawnBoss()
    {
        if (bossPrefab != null)
        {
            Vector3 spawnPos = bossSpawnPoint != null
                ? bossSpawnPoint.position
                : transform.position;

            bossRef = Instantiate(bossPrefab, spawnPos, Quaternion.identity);
            bossDoor.SetBoss(bossRef);
            bossSpawned = true;

            bossHealthBarMaterial.SetFloat("_Fill", 1);

            BossUI.Show();
            BossUI.ShowFlyBossText();
        }
        else
        {
            Debug.LogWarning("Boss prefab not assigned!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            // GarbageMessageContainer.SetActive(true);
            cg = GarbageMessageContainer.GetComponent<CanvasGroup>();
            cg.alpha = 1;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            //GarbageMessageContainer.SetActive(false);
            cg = GarbageMessageContainer.GetComponent<CanvasGroup>();
            cg.alpha = 0;
        }
    }
}
