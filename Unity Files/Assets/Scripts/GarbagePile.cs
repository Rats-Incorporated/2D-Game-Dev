using UnityEngine;

public class garbage_pile : MonoBehaviour
{
    public float eatTimeRequired = 5f;   // total time needed to eat
    private float currentEatTime = 0f;   // current eat time
    private bool playerInRange = false;  // is player inside trigger?


    public GameObject bossPrefab;        // boss to spawn
    public Transform bossSpawnPoint;     // where boss appears
    private bool bossSpawned = false;    // prevent double spawn

    void Update()
    {
        if (playerInRange && !bossSpawned)
        {
            if (Input.GetKey(KeyCode.E))
            {
                currentEatTime += Time.deltaTime;

                Debug.Log("Eating progress: " + currentEatTime + " / " + eatTimeRequired);

                if (currentEatTime >= eatTimeRequired)
                {
                    SpawnBoss();
                    Destroy(gameObject);
                }
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

            Instantiate(bossPrefab, spawnPos, Quaternion.identity);
            bossSpawned = true;
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
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
