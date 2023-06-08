using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HealHelper
{
    public static Color[] InfusionLookup = { Color.green };
}

//Healing
public interface IHealable
{
    bool Heal(HealInfo healInfo);
}

[System.Serializable]
public struct HealInfo
{
    public float amount;
    public E_Infusion infusion;
}

[System.Serializable]
public enum E_Infusion
{
    None
}

//Marking
public interface IMarkable
{
    bool Mark(MarkInfo markInfo);
}

[System.Serializable]
public struct MarkInfo
{
    public float amount;
}

