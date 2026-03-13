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
    public Text cooldownText;  // use Text if not using TMP

    [Header("References")]
    public PlayerController playerController;

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
        // Determine direction based on player facing
        Vector2 direction = playerController.isFacingRight ? Vector2.right : Vector2.left;

        // Flip firePoint X offset based on facing
        Vector3 localOffset = firePoint.localPosition;
        localOffset.x = Mathf.Abs(localOffset.x) * (playerController.isFacingRight ? 1 : -1);

        // Calculate spawn position in world space
        Vector3 spawnPos = playerController.transform.position + localOffset;

        // Spawn the shuriken
        GameObject shuriObj = Instantiate(shurikenPrefab, spawnPos, Quaternion.identity);
        Shuriken shuri = shuriObj.GetComponent<Shuriken>();

        // Get player velocity to add to the shuriken
        Vector2 playerVel = playerController.GetPlayerVector();
        shuri.Initialize(direction, new Vector2(playerVel.x, 0)); // only horizontal component
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