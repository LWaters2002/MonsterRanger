using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackComponent : MonoBehaviour
{
    public string attackName;
    public UnityEvent OnAttack;

    protected Entity _entity;

    public System.Action OnAttackComplete;

    public virtual void Init(Entity entity)
    {
        _entity = entity;
    }

    public virtual void StartAttack()
    {
        _entity.animator.CrossFade(attackName, .25f);
    }

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
