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

    private bool _active;

    public float exhaustAmount;
    
    [Header("Effectiveness")]
    public float minimumRange;
    public float maximumRange;

    public float cooldown;
    private float _cooldown;

    public float weight;


    public virtual void Init(Entity entity)
    {
        _entity = entity;
    }

    public virtual void StartAttack()
    {
        _entity.animator.CrossFade(attackName, .25f);
        _active = true;
        _cooldown = cooldown;
    }

    public virtual void Update()
    {
        if (_cooldown > 0.0f && !_active)
        {
            _cooldown -= Time.deltaTime;
        }
    }

    public void AttackComplete()
    {
        _active = false;
        OnAttackComplete?.Invoke();
    }

    public virtual void Attack(int attackStep)
    {

        OnAttack?.Invoke();
    }

    public virtual float CalculateEffectiveness()
    {
        if (_cooldown > 0.0f) return 0.0f;

        float dist = _entity.blackboard.DistanceToTarget;

        if (dist < minimumRange || dist > maximumRange) return 0.0f;

        return 1.0f * weight;
    }
}
