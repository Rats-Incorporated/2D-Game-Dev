using System;
using UnityEngine;

public class ScorpionBossHP : MonoBehaviour
{
    public Material bossHealthBarMaterial;
    public BossUI BossUI;
    void Start()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            bossHealthBarMaterial.SetFloat("_Fill", 1);
            BossUI.Show();
            BossUI.ShowScorpionBossText();
        }
    }
}
