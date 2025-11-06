using UnityEngine;

public class Player_Teleport : MonoBehaviour
{
    private GameObject currentTeleporter;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentTeleporter != null)
        {
            Vector3 teleportLocation = currentTeleporter.GetComponent<Teleporter>().GetDestination().position;
            transform.position = teleportLocation;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Teleporter"))
        {
            currentTeleporter = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == currentTeleporter)
        {
            currentTeleporter = null;
        }
    }

}
