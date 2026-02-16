using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DamageFlash : MonoBehaviour
{
    [Header("Flash Settings")]
    public float flashDuration = 0.15f;
    public float dimAlpha = 0.5f;

    private SpriteRenderer sr;
    private Color originalColor;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    public void Flash()
    {
        if (!gameObject.activeInHierarchy) return;
        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        // Bright white
        sr.color = new Color(1f, 0.4f, 0.4f);
        yield return new WaitForSeconds(flashDuration / 2f);

        // Slight dim
        sr.color = new Color(1f, 1f, 1f, dimAlpha);
        yield return new WaitForSeconds(flashDuration / 2f);

        // Restore
        sr.color = originalColor;
    }
}