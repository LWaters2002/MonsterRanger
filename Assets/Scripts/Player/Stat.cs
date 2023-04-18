using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    private float _value;
    private float _rawValue;

    public float Value { get { return _value; } }

    private List<StatModifier> _modifiers;

    public Stat(float value)
    {
        _modifiers = new List<StatModifier>();

        _rawValue = value;
        _value = value;
    }

    public void AddModifier(StatModifier modifier)
    {
        if (modifier == null) return;

        _modifiers.Add(modifier);
        CalculateValue();
    }

    public void RemoveModifier(StatModifier modifier)
    {
        if (modifier == null) return;

        _modifiers.Remove(modifier);
        CalculateValue();
    }

    private void CalculateValue()
    {
        float calculatedValue = _rawValue;
        float multiplier = 1.0f;

        foreach (StatModifier modifier in _modifiers)
        {
            float amount = modifier.amount;

            if (modifier.isPercent)
            {
                multiplier += amount;
                continue;
            }

            calculatedValue += _rawValue;
        }

        _value = calculatedValue * multiplier;
    }
}

[System.Serializable]
public class StatModifier
{
    public float amount;
    public bool isPercent;

    public StatModifier(float amount, bool isPercent)
    {
        this.amount = amount;
        this.isPercent = isPercent;
    }
}
