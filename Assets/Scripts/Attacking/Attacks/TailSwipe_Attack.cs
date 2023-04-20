using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Hurtbox))]
public class TailSwipe_Attack : AttackComponent
{
    [SerializeField]
    private Hurtbox _hurtbox;

    public override void Attack(int attackStep)
    {
        base.Attack(attackStep);

        switch (attackStep)
        {
            case 0:
                _hurtbox.ActivateColliders();
                break;
            case 1:
                _hurtbox.DeactivateColliders();
                break;
        }
    }

    public override float CalculateEffectiveness()
    {
        return 1.0f;
    }
}

