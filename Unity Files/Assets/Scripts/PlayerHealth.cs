using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    public float maxHealth = 100f;
    public float currentHealth;
    public Image healthFill;
    public LogicScript Logic;
    PlayerInvulnerability invuln;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }
    void Awake()
    {
        invuln = GetComponent<PlayerInvulnerability>();
    }
    public void TakeDamage(float amount)
    {
        if (invuln != null && invuln.IsInvulnerable())
            return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // prevents negative damage
        if (invuln != null)
            invuln.TriggerInvulnerability();

        UpdateHealthUI();

    }

    void UpdateHealthUI()
    {
        healthFill.fillAmount = currentHealth / maxHealth;
        if (currentHealth <= 0)
        {
            Logic.LoseGame();
        }
    }
    // Update is called once per frame
    void Update()
    {

    }


}
