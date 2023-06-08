using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Hurtbox))]
public class TailSwipe_Attack : Attack_Gallant
{
    [SerializeField]
    private Hurtbox _hurtbox;

    [Header("Attack Settings")]
    public float turnSpeed;
    public float turnDuration;

    public float maxWalkTime = 1f;
    public float speed = 5;
    public float acceptanceRadius;


    public override void StartAttack()
    {
        StartCoroutine(_entity.WalkToTarget(base.StartAttack, maxWalkTime, acceptanceRadius, speed));
    }

    public override void Attack(int attackStep)
    {
        base.Attack(attackStep);

        StartCoroutine(FaceTarget());

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

    public IEnumerator FaceTarget()
    {
        float t = 0.0f;

        while (t < turnDuration)
        {
            t += Time.deltaTime;
            _entity.TurnToTargetTicked(turnSpeed);
            yield return null;
        }

        //  base.StartAttack();
    }





}

