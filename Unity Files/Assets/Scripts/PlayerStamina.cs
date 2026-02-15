using UnityEngine;
using UnityEngine.UI;

public class PlayerStamina : MonoBehaviour
{

    public float maxStamina = 100f;
    public float currentStamina;
    public Image staminaFill;

    public float staminaRegenRate = 10f; // per second
    public bool staminaRegenLock = true; // letting dash controller handle setting this

    void Start()
    {
        currentStamina = maxStamina;
    }

    public void UseStamina(float amount)
    {
        currentStamina -= amount;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina); // this shouldn't ever trigger, but just in case
        UpdateStaminaUI();
    }

    void UpdateStaminaUI()
    {
        staminaFill.fillAmount = currentStamina / maxStamina;
    }

    public void StaminaRegen()
    {
        if (!staminaRegenLock && currentStamina <= maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina); // ensuring it doesnt go over max
            UpdateStaminaUI();
        }
    }

    void Update()
    {

    }
}
