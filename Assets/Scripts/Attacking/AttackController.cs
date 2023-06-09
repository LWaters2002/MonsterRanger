using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AttackController : MonoBehaviour
{

    [SerializeField]
    private AttackComponent _attack;
    private AttackComponent[] _attacks;

    public float effectiveThreshold;

    private Entity _entity;

    private float _exhaust = 0.0f;
    public float exhaustCap;
    public float exhaustTime;

    public void Init(Entity entity)
    {
        _entity = entity;
        _attacks = GetComponentsInChildren<AttackComponent>();

        foreach (AttackComponent attack in _attacks)
        {
            attack.Init(_entity);
        }
    }

    public bool CheckExhaust()
    {
        if (_exhaust < exhaustCap) return false;

        StartCoroutine(LargeExhaust());

        return true;
    }

    private IEnumerator LargeExhaust()
    {
        _entity.animator.SetBool("Exhausted", true);
        _entity.animator.CrossFade("LargeExhaust", .2f);

        yield return new WaitForSeconds(exhaustTime);

        _entity.animator.SetBool("Exhausted", false);
        _exhaust = 0;

        ChooseAttack();
    }

    public void SetEnabledAllAttacks(bool isEnabled)
    {
        foreach (AttackComponent attack in _attacks)
        {
            attack.StopAttack();
        }
    }

    public void ChooseAttack()
    {
        if (!this.enabled) return;
        if (!_entity.canAttack) return;
        if (CheckExhaust()) return;

        AttackComponent idealAttack = null;
        float highestScore = 0.0f;

        List<AttackComponent> effectiveAttacks = new List<AttackComponent>();

        foreach (AttackComponent attack in _attacks)
        {
            if (!attack.isEnabled) continue;

            float effectiveness = attack.CalculateEffectiveness();

            if (effectiveness == 0.0f) continue;

            if (effectiveness > highestScore)
            {
                highestScore = effectiveness;
                idealAttack = attack;
            }

            if (effectiveness > effectiveThreshold) effectiveAttacks.Add(attack);
        }

        int count = effectiveAttacks.Count;
        int randomR = Random.Range(0, _attacks.Length);
        if (count == 0) { SetAttack(_attacks[randomR]); return; }

        int random = Random.Range(0, count);

        SetAttack(effectiveAttacks[random]);
    }

    public void CompleteAttack()
    {
        _attack?.AttackComplete();
    }

    private void SetAttack(AttackComponent attack)
    {
        if (_attack) _attack.OnAttackComplete -= ChooseAttack;

        _attack = attack;

        _attack.OnAttackComplete += ChooseAttack;

        Debug.Log(_attack.attackName + " Attack has started!");
        _exhaust += _attack.exhaustAmount;

        _attack.StartAttack();
    }

    public AttackComponent SelectAttack(string attackName)
    {
        _attack = _attacks.Where(x => x.attackName == attackName).First();

        _attack.StartAttack();
        return _attack;
    }

    public void SelectAttackCombat(string attackName)
    {
        AttackComponent stringAttack = _attacks.Where(x => x.attackName == attackName).First();

        SetAttack(stringAttack);
    }


    public void Attack(int attackStep)
    {
        if (!_attack) return;
        if (!_attack._active) return;

        _attack?.Attack(attackStep);
    }
}
