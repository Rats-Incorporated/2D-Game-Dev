using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerInvulnerability : MonoBehaviour
{
    [Header("Invulnerability Settings")]
    public float invulnerabilityDuration = 1.0f;
    public int flashCount = 8;
    public float dimAlpha = 0.5f;

    SpriteRenderer sr;
    Color originalColor;
    bool invulnerable;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    public bool IsInvulnerable()
    {
        return invulnerable;
    }

    public void TriggerInvulnerability()
    {
        if (!gameObject.activeInHierarchy) return;
        StartCoroutine(InvulnerabilityRoutine());
    }

    IEnumerator InvulnerabilityRoutine()
    {
        invulnerable = true;

        float flashInterval = invulnerabilityDuration / (flashCount * 2f);

        for (int i = 0; i < flashCount; i++)
        {
            // Flash white
            sr.color = Color.white;
            yield return new WaitForSeconds(flashInterval);

            // Dim
            sr.color = new Color(1f, 1f, 1f, dimAlpha);
            yield return new WaitForSeconds(flashInterval);
        }

        sr.color = originalColor;
        invulnerable = false;
    }
}
