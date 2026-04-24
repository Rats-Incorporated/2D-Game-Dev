using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerInvulnerability))]
public class PlayerInvulnerability : MonoBehaviour
{
    public float invulnerabilityDuration = 1.0f;
    public int flashCount = 8;
    public float dimAlpha = 0.5f;

    public SpriteRenderer playerSpriteRenderer; // assign in inspector

    private Color originalColor;
    private bool invulnerable;

    void Awake()
    {
        if (playerSpriteRenderer == null)
        {
            Debug.LogWarning("PlayerSpriteRenderer not assigned! Attempting to find child 'PlayerSprite'...");
            Transform child = transform.Find("PlayerSprite");
            if (child != null)
                playerSpriteRenderer = child.GetComponent<SpriteRenderer>();
        }

        if (playerSpriteRenderer != null)
            originalColor = playerSpriteRenderer.color;
        else
            Debug.LogError("No SpriteRenderer found for invulnerability flashing!");
    }

    public bool IsInvulnerable()
    {
        return invulnerable;
    }


    public void TriggerInvulnerability(float duration)
    {
        if (!gameObject.activeInHierarchy) return;
        StartCoroutine(InvulnerabilityRoutine(duration));
    }

    public void TriggerInvulnerability()
    {
        TriggerInvulnerability(invulnerabilityDuration);
    }

    public void TriggerInvulnerabilityPermaOn()
    {
        invulnerable = true;
    }
    public void TriggerInvulnerabilityPermaOff()
    {
        invulnerable = false;
    }

    IEnumerator InvulnerabilityRoutine(float duration)
    {
        if (playerSpriteRenderer == null) yield break;

        invulnerable = true;

        float flashInterval = duration / (flashCount * 2f);

        for (int i = 0; i < flashCount; i++)
        {
            // Flash white
            playerSpriteRenderer.color = Color.white;
            yield return new WaitForSeconds(flashInterval);

            // Dim
            playerSpriteRenderer.color = new Color(1f, 1f, 1f, dimAlpha);
            yield return new WaitForSeconds(flashInterval);
        }

        // Restore original color
        playerSpriteRenderer.color = originalColor;
        invulnerable = false;
    }
}