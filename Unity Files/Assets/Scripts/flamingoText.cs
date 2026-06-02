using UnityEngine;

public class flamingoText : MonoBehaviour
{

    [Header("objs")]
    public GameObject TextBubble1;
    public GameObject flamingoTrigger;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (TextBubble1 != null)
            TextBubble1.SetActive(false);
    }

    // Update is called once per frame
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
