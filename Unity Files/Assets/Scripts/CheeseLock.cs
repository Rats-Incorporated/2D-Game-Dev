using UnityEngine;
using System.Collections;

public class CheeseLock : MonoBehaviour
{
    public GameObject bubble;
    private bool showing = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController pc = collision.GetComponentInParent<PlayerController>();
            if (pc.Logic.pickedUpCollectables < 3 && !showing)
            {
                StartCoroutine(ShowBubble());
            }
            else if (pc.Logic.pickedUpCollectables == 3)
            {
                GameObject.Destroy(gameObject);
            }
        }

    }

    IEnumerator ShowBubble()
    {
        showing = true;

        bubble.SetActive(true);

        yield return new WaitForSeconds(3f);

        bubble.SetActive(false);

        showing = false;
    }
}
