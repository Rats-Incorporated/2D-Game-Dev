using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    public float maxHealth = 100f;
    public float currentHealth;
    public Image healthFill;
    public LogicScript Logic;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // prevents negative damage
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
