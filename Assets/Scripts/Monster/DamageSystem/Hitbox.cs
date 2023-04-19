using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hitbox : MonoBehaviour, IMarkable, IHealable
{
    public string partName;

    public float healRequired;
    private float _healAmount = 0f;
    public float markRequired;
    private float _markAmount = 0f;

    public UnityEvent OnFullMarked;
    public UnityEvent OnFullHeal;

    public bool isHealed { get; private set; }
    public bool isMarked { get; private set; }

    public System.Action<string, float, float> OnHeal;
    public System.Action<string, float, float> OnMark;

    public bool Heal(HealInfo healInfo)
    {
        if (isHealed) return false;

        _healAmount += healInfo.amount;

        if (_healAmount >= healRequired)
        {
            isHealed = true;
            _healAmount = healRequired;
            Debug.Log(partName + " Fully Healed!");
            OnFullHeal?.Invoke();
        }

        OnHeal?.Invoke(partName, _healAmount, healRequired);
        
        return true;
    }

    public bool Mark(MarkInfo markInfo)
    {
        if (isMarked) return false;

        _markAmount += markInfo.amount;

        if (_markAmount >= markRequired)
        {
            isMarked = true;
            _markAmount = markRequired;
            OnFullMarked?.Invoke();
        }

        OnMark?.Invoke(partName, _markAmount, markRequired);

        return true;
    }

}




