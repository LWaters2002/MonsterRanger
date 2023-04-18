using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackComponent : MonoBehaviour
{
    public string attackName;
    public UnityEvent OnAttack;

    public virtual void Attack(int attackStep)
    {
        Debug.Log("Fired Attack : " + attackName);

        OnAttack?.Invoke();
    }

    public virtual float CalculateEffectiveness()
    {
        return 1.0f;
    }
}
