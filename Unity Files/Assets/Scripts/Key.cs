using UnityEngine;
using UnityEngine.UI;

public class key : MonoBehaviour
{
    public GameObject InventoryKey;
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
            collision.GetComponent<PlayerController>().PickupKey();
            InventoryKey.SetActive(true);
            Destroy(gameObject);
        }
    }
}
