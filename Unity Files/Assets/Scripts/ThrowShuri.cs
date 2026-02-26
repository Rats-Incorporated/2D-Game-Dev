using UnityEngine;
using UnityEngine.UI;

public class PlayerShuriken : MonoBehaviour
{
    [Header("Projectile")]
    public GameObject shurikenPrefab;
    public Transform firePoint;

    [Header("Cooldown")]
    public float cooldown = 3f;
    private float cooldownTimer = 0f;

    [Header("UI")]
    public Image cooldownOverlay;
    public Text cooldownText;  // change to Text if not using TMP

    void Update()
    {
        cooldownTimer += Time.deltaTime;

        HandleCooldownUI();

        if (Input.GetKeyDown(KeyCode.K) && cooldownTimer >= cooldown)
        {
            ThrowShuriken();
            cooldownTimer = 0f;
        }
    }

    void ThrowShuriken()
    {
        Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        GameObject shuriObj = Instantiate(shurikenPrefab, firePoint.position, Quaternion.identity);

        Shuriken shuri = shuriObj.GetComponent<Shuriken>();
        shuri.Initialize(-direction);

    }

    void HandleCooldownUI()
    {
        if (cooldownOverlay == null) return;

        if (cooldownTimer < cooldown)
        {
            float percentRemaining = 1 - (cooldownTimer / cooldown);
            cooldownOverlay.fillAmount = percentRemaining;

            // Dim icon
            cooldownOverlay.color = new Color(0, 0, 0, 0.6f);

            // Countdown number
            if (cooldownText != null)
            {
                float timeRemaining = cooldown - cooldownTimer;
                cooldownText.text = Mathf.Ceil(timeRemaining).ToString();
                cooldownText.gameObject.SetActive(true);
            }
        }
        else
        {
            cooldownOverlay.fillAmount = 0f;

            if (cooldownText != null)
            {
                cooldownText.gameObject.SetActive(false);
            }
        }
    }
}