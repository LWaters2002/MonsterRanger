using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AttackController : MonoBehaviour
{

    [SerializeField]
    private AttackComponent _attack;
    private AttackComponent[] _attacks;

    private Entity _entity;

    public void Init(Entity entity)
    {
        _entity = entity;
        _attacks = GetComponentsInChildren<AttackComponent>();

        foreach (AttackComponent attack in _attacks)
        {
            attack.Init(_entity);
        }
    }

    private void DelayStart()
    {
        _attack.StartAttack();
    }

    public AttackComponent SelectAttack(string attackName)
    {
        _attack = _attacks.Where(x => x.attackName == attackName).First();

        Debug.Log(_attack.attackName + " Attack has started!");
        _attack.StartAttack();
        return _attack;
    }

    public void Attack(int attackStep)
    {
        _attack?.Attack(attackStep);
    }
}
