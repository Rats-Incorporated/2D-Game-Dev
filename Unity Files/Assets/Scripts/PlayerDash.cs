using System.Data;
using UnityEngine;
using System;

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

    void Start()
    {
        dashTotal = player.dashTotal;
        dashCount = dashTotal;
    }

    void Update()
    {
        CheckCD();
        CheckGCD();
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
    }

    public int GetDashCount()
    {
        return dashCount;
    }

    public float GetTimeToCD()
    {
        return dashCD - cdTimer;
    }
}
