using UnityEngine;


public class garbage_pile : MonoBehaviour
{
    public float eatTimeRequired = 5f;  // Total time needed to eat
    private float currentEatTime = 0f;  // Accumulated eat time
    private bool playerInRange = false; // Is player inside trigger?

    void Update()
    {
        if (playerInRange)
        {

            if (Input.GetKey(KeyCode.E))
            {
                currentEatTime += Time.deltaTime;


                Debug.Log("Eating progress: " + currentEatTime + " / " + eatTimeRequired);


                if (currentEatTime >= eatTimeRequired)
                {
                    Destroy(gameObject);
                }
            }
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


