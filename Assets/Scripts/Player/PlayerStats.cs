using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats
{
    private float _health;
    private float _maxHealth;

    private float _stamina;
    private float _maxStamina;

    private float _experience;
    private float _maxExperience;

    private PlayerCharacter _player;

    public System.Action<float, float> OnHealthChange;
    public System.Action<float, float> OnStaminaChange;
    public System.Action<float, float> OnExperienceChange;

    public System.Action OnDeath;

    private float _staminaBreak = .2f;
    private float _staminaRegenRate = 25f;

    private Coroutine _regenCoroutine;

    public PlayerStats(PlayerCharacter player, float health, float stamina, float experience)
    {
        _player = player;
        _health = health;
        _maxHealth = health;
        _stamina = stamina;
        _maxStamina = stamina;
        _maxExperience = experience;
    }

    public void AlterHealth(float amount)
    {
        _health += amount;
        _health = Mathf.Clamp(_health, 0f, _maxHealth);
        OnHealthChange.Invoke(_health, _maxHealth);
        if (_health <= 0f) { OnDeath.Invoke(); }
    }

    IEnumerator RegenStamina()
    {
        yield return new WaitForSeconds(_staminaBreak);

        while (_stamina < _maxStamina)
        {
            AlterStamina(_staminaRegenRate * Time.deltaTime);
            yield return null;
        }
    }

    public bool ConsumeStamina(float amount)
    {
        if ((_stamina - amount) <= 0) return false;
        if (_regenCoroutine != null) _player.StopCoroutine(_regenCoroutine);

        _regenCoroutine = _player.StartCoroutine(RegenStamina());
        
        AlterStamina(-amount);
        
        return true;
    }

    public void AlterStamina(float amount)
    {
        _stamina += amount;
        _stamina = Mathf.Clamp(_stamina, 0f, _maxStamina);
        OnStaminaChange?.Invoke(_stamina, _maxStamina);
    }


}
