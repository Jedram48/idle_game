using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar_Script : MonoBehaviour
{
    public Spawner spawner;
    [SerializeField] public Image healthBar;
    public TextMeshProUGUI healthBarText;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize healthBarText
        healthBarText = GetComponentInChildren<TextMeshProUGUI>();
        // healthBar = GetComponentInChildren<Image>();

        // Check if healthBarText is not null before using it
        if (healthBarText != null)
        {
            healthBar.fillAmount = (float)spawner.enemy.Hp / (float)spawner.enemy.maxHp;
            healthBarText.text = spawner.enemy.Hp.ToString();
        }
        else
        {
            Debug.LogError("TextMeshProUGUI component not found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if healthBarText is not null before using it
        if (healthBarText != null)
        {
            healthBar.fillAmount = (float)spawner.enemy.Hp / (float)spawner.enemy.maxHp;
            healthBarText.text = spawner.enemy.Hp.ToString();
        }
    }
}
