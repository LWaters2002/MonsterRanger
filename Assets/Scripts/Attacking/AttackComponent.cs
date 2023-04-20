using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackComponent : MonoBehaviour
{
    public string attackName;
    public UnityEvent OnAttack;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponentInParent<Animator>();
    }

    public virtual void StartAttack()
    {
        _animator.CrossFade(attackName, .25f);
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
