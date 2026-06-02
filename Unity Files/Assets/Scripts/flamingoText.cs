using UnityEngine;

public class flamingoText : MonoBehaviour
{
    [Header("Objsects")]
    public GameObject TextBubble1;
    public GameObject flamingoTrigger;

    void Start()
    {
        // Hide the text bubble at the start
        if (TextBubble1 != null)
            TextBubble1.SetActive(false);
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (TextBubble1 != null)
                TextBubble1.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (TextBubble1 != null)
                TextBubble1.SetActive(false);
        }
    }
}