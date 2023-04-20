using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AttackController : MonoBehaviour
{

    [SerializeField]
    private AttackComponent _attack;
    private AttackComponent[] _attacks;

    public void Init()
    {
        _attacks = GetComponentsInChildren<AttackComponent>();
    }

    private void Start()
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
