using System.Data;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayerDash : MonoBehaviour
{

    public PlayerController player;

    // modifiable vars
    public float dashCD = 3.0f; // total cooldown for the dash to come back
    public float dashGCD = 0.6f; // global cooldown for using dashes sequentially

    // sharing vars
    public bool inGCD = false;

    // private vars
    private int dashTotal;
    private int dashCount;
    private float cdTimer = 0.0f;
    private float gcdTimer = 0.0f;

    // stamina regen
    private float staminaRegenRate = 0.5f / 3f; // 50% over 3 seconds
    private bool regeneratingStamina = false;

    public Image stamFill;

    void Start()
    {
        dashTotal = player.dashTotal;
        dashCount = dashTotal;
    }

    void Update()
    {
        CheckCD();
        CheckGCD();
        RegenStamina();
        Debug.Log(stamFill.fillAmount);
        // print(cdTimer);
    }

    private void CheckCD()
    {
        if (dashCount < dashTotal)
        {
            cdTimer += Time.deltaTime;
            if (cdTimer >= dashCD)
            {
                dashCount++;
                cdTimer = 0.0f;
            }
        }
    }

    private void CheckGCD()
    {
        if (inGCD)
        {
            gcdTimer += Time.deltaTime;
            if (gcdTimer >= dashGCD)
            {
                inGCD = false;
            }
        }
    }

    public void SetDashVars()
    {
        dashCount--;
        gcdTimer = 0.0f;
        inGCD = true;

        stamFill.fillAmount -= 0.5f;              // subtract 50%
        stamFill.fillAmount = Mathf.Clamp01(stamFill.fillAmount);

        regeneratingStamina = true;               // begin regen
    }

    public int GetDashCount()
    {
        return dashCount;
    }

    public float GetTimeToCD()
    {
        return dashCD - cdTimer;
    }

    private void RegenStamina()
    {
        if (regeneratingStamina && stamFill.fillAmount < 1f)
        {
            stamFill.fillAmount += staminaRegenRate * Time.deltaTime;

            if (stamFill.fillAmount >= 1f)
            {
                stamFill.fillAmount = 1f;
                regeneratingStamina = false;
            }
        }
    }
}
