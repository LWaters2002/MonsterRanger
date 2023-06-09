using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JabAttack_Gallant : Attack_Gallant
{
    [SerializeField]
    private Hurtbox _hurtbox;

    [Header("Attack Settings")]
    public float turnSpeed;
    public float turnDuration;

    public float maxWalkTime = 1f;
    public float speed = 8;
    public float acceptanceRadius;

    private bool _turning = false;

    public override void StartAttack()
    {
        StartCoroutine(_entity.WalkToTarget(base.StartAttack, maxWalkTime, acceptanceRadius, speed));
        _turning = true;
    }

    public override void Attack(int attackStep)
    {
        base.Attack(attackStep);

        StartCoroutine(Turn());

        switch (attackStep)
        {
            case 0:
                _hurtbox.ActivateColliders();
                break;
            case 1:
                _turning = false;
                _hurtbox.DeactivateColliders();
                break;
        }
    }

    public override void StopAttack()
    {
        base.StopAttack();
        _turning = false;
    }

    private IEnumerator Turn()
    {
        while (_turning)
        {
            _entity.TurnToTargetTicked(turnSpeed);
            yield return null;
        }
    }

}
