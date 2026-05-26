using UnityEngine;

public class JumpPadBuzz : MonoBehaviour
{
    public AudioClip buzzSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AudioController.Instance.PlaySFX(buzzSound);
        }
    }
}