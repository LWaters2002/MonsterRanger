using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LUI;
using DG.Tweening;
using System;

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

        GameManager.Get().OnCutsceneChange += ChangeVisibility;
    }

    private void ChangeVisibility(bool obj)
    {
        gameObject.SetActive(!obj);
    }

    private void UpdateHealthBar(float health, float maxHealth)
    {
        float percent = health / maxHealth;

        DOTween.To(() => healthBar.fillAmount, x => healthBar.fillAmount = x, percent, .15f).SetEase(Ease.OutSine);

    }

    private void UpdateStaminaBar(float stamina, float maxStamina)
    {
        float percent = stamina / maxStamina;

        DOTween.To(() => staminaBar.fillAmount, x => staminaBar.fillAmount = x, percent, .15f).SetEase(Ease.OutSine);
    }

}
