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

        Invoke("DelayStart", 2f);
    }

    private void DelayStart()
    {
        _attack.StartAttack();                       
    }

    public bool SelectAttack(System.Type type)
    {
        _attack = _attacks.Where(x => x.GetType() == type).First();

        _attack.StartAttack();
        return _attack;
    }

    public void Attack(int attackStep)
    {
        Debug.Log(_attack.attackName);
        _attack?.Attack(attackStep);
    }
}
