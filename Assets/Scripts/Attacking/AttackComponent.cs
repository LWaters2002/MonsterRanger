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

    public virtual void Init(Entity entity)
    {
        _entity = entity;
    }

    public virtual void StartAttack()
    {
        _entity.animator.CrossFade(attackName, .25f);
        _active = true;
    }

    public virtual void Update()
    {
        // if (!_active) return;

        // AnimatorStateInfo info = _entity.animator.GetCurrentAnimatorStateInfo(0);

        // if ((info.length > info.normalizedTime) && info.IsName(attackName))
        // {
        //     AttackComplete();
        // }
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
        return 1.0f;
    }
}
