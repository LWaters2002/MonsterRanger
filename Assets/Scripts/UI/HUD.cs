using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LUI;

public class HUD : PlayerUI
{
    private PlayerStats _playerStats;

    [SerializeField]
    private Image healthBar;
    [SerializeField]
    private Image staminaBar;

    public override void Init(PlayerCharacter playerCharacter)
    {
        _playerStats = playerCharacter.stats;

        healthBar.fillAmount = 1f;
        staminaBar.fillAmount = 1f;
        
        _playerStats.OnHealthChange += UpdateHealthBar;
        _playerStats.OnStaminaChange += UpdateStaminaBar;
    }

    private void UpdateHealthBar(float health, float maxHealth)
    {
        healthBar.fillAmount = health / maxHealth;
    }

    private void UpdateStaminaBar(float stamina, float maxStamina)
    {
        staminaBar.fillAmount = stamina / maxStamina;
    }

}
