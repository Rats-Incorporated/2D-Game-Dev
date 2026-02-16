using System.Data;
using UnityEngine;
using System;

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

    void Start()
    {
        //dashTotal = player.dashTotal;
        //dashCount = dashTotal;
    }

    void Update()
    {
        //CheckCD();
        CheckGCD();
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

    //public int GetDashCount()
    //{
    //    return dashCount;
    //}

    //public float GetTimeToCD()
    //{
    //    return dashCD - cdTimer;
    //}
}
