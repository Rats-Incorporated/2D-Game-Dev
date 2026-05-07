using System.Data;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayerDash : MonoBehaviour
{

    public PlayerController player;

    // commenting out old cooldown code! this is incase we ever need to revert back
    // to a cooldown system presuming we need stamina / exp / or whichever for something else
    // or if needed for a different movement module ^^

    // modifiable vars
    //public float dashCD = 3.0f; // total cooldown for the dash to come back
    public float dashGCD = 0.6f; // global cooldown for using dashes sequentially
    public float stamCD = 1.2f;
    public float stamCost = 40f; // how much stamina a dash costs

    // sharing vars
    public bool inGCD = false;
    public bool inStam = false;

    // private vars
    //private int dashTotal;
    //private int dashCount;
    //private float cdTimer = 0.0f;
    private float gcdTimer = 0.0f;
    private float stamTimer = 0.0f;

    [Header("UI")]
    public Image DashOverlay;
    public Text DashText;

    void Start()
    {
        //dashTotal = player.dashTotal;
        //dashCount = dashTotal;
    }

    void Update()
    {
        //CheckCD();
        CheckGCD();
        UpdateDashUI();
    }

    //private void CheckCD()
    //{
    //    if (dashCount < dashTotal)
    //    {
    //        cdTimer += Time.deltaTime;
    //        if (cdTimer >= dashCD)
    //        {
    //            dashCount++;
    //            cdTimer = 0.0f;
    //        }
    //    } 
    //}

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

        if (inStam)
        {
            stamTimer += Time.deltaTime;
            if (stamTimer >= stamCD)
            {
                inStam = false;
            }
        }
    }

    public void SetDashVars()
    {
        //dashCount--;
        gcdTimer = 0.0f;
        stamTimer = 0.0f;
        player.StaminaState.UseStamina(stamCost);
        inGCD = true;
        inStam = true;
    }

    private void UpdateDashUI()
    {
        if (DashOverlay == null) return;

        float percent = inGCD ? (1 - (gcdTimer / dashGCD)) : 0f;

        DashOverlay.fillAmount = percent;
        DashOverlay.color = new Color(0, 0, 0, 0.6f);

        if (DashText != null)
        {
            if (inGCD)
            {
                DashText.gameObject.SetActive(true);
                DashText.text = Mathf.Ceil(dashGCD - gcdTimer).ToString();
            }
            else
            {
                DashText.gameObject.SetActive(false);
            }
        }
    }

    //public int GetDashCount()
    //{
    //    return dashCount;
    //}

    //public float GetTimeToCD()
    //{
    //    return dashCD - cdTimer;
    //}
}
