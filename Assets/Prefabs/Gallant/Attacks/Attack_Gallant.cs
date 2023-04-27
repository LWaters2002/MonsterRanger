using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Gallant : AttackComponent
{
    protected Gallant_Entity _gallant;

    public override void Init(Entity entity)
    {
        base.Init(entity);
        
        _gallant = (Gallant_Entity)entity;
    }

    public override void Attack(int attackStep)
    {
        base.Attack(attackStep);
    }

    public override float CalculateEffectiveness()
    {
        return base.CalculateEffectiveness();
    }

    public override bool Equals(object other)
    {
        return base.Equals(other);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override void StartAttack()
    {
        base.StartAttack();
    }

}
