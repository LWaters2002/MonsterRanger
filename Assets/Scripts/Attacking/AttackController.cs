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

    public bool SelectAttack(System.Type type)
    {
        _attack = _attacks.Where(x => x.GetType() == type).First();

        return _attack;
    }

    public void Attack()
    {
        _attack?.Attack();
    }
}
