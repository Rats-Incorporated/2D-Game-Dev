using UnityEngine;

public class DesertEndTriggerCheese : MonoBehaviour
{
    public GameObject cheese;

    public Vector3 startPos = new Vector3(155.99f, -61.26f, 0f);
    public Vector3 endPos = new Vector3(137.67f, -61.26f, 0f);

    public float eatTimeRequired = 3.8f;   // total time needed to eat
    private float currentEatTime = 0f;   // current eat time
    private bool playerInRange = false;  // is player inside trigger?

    public GameObject GarbageMessageContainer;
    public GameObject GarbageLoadingBarContainer;
    public GameObject GarbageLoadingBar;
    private CanvasGroup cg;

    public LogicScript Logic;

    private float timer = 0f;
    private float totalTime = 6.7f;
    private bool endStart = false;
    private bool endEnd = false;

    void Start()
    {
        cg = GarbageMessageContainer.GetComponent<CanvasGroup>();
        cg.alpha = 0;
        cheese.transform.position = startPos;
    }

    public void StartEnd()
    {
        endStart = true;
        endEnd = false;
        timer = 0f;
    }

    void Update()
    {
        if (endStart && playerInRange) 
        { 
            if (Input.GetButton("Interact"))
            {
                currentEatTime += Time.deltaTime;

                if (currentEatTime > 0)
                {
                    GarbageLoadingBarContainer.SetActive(true);
                    float eatTimePercent = currentEatTime / eatTimeRequired;
                    GarbageLoadingBar.GetComponent<RectTransform>().localScale = new Vector3(eatTimePercent * 1.0f, 1f, 1f);
                }

                if (currentEatTime >= eatTimeRequired)
                {
                    Logic.WinGame();
                }
            }
        }

        if (!endStart || endEnd)
            return;

        timer += Time.deltaTime;

        // timer from 0 to 1
        float t = timer / totalTime;
        t = Mathf.Clamp01(t);

        // ease-out
        t = 1f - Mathf.Pow(1f - t, 2f);

        // moving the cheese
        cheese.transform.position = Vector3.Lerp(startPos, endPos, t);

        // stop when done
        if (t >= 1f)
        {
            endEnd = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            cg = GarbageMessageContainer.GetComponent<CanvasGroup>();
            cg.alpha = 1;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            cg = GarbageMessageContainer.GetComponent<CanvasGroup>();
            cg.alpha = 0;
        }
    }
}