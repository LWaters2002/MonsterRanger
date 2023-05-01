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

    public override void StartAttack()
    {
        StartCoroutine(WalkToPlayer());
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

        base.StartAttack();
    }


    public IEnumerator WalkToPlayer()
    {
        Vector3 location = Vector3.Lerp(_entity.transform.position, _entity.blackboard.Target.transform.position, .8f);

        _entity.agent.SetDestination(location);
        _entity.agent.isStopped = false;
        _entity.agent.speed = 7f;
        float t = 0;

        while (Vector3.Distance(_entity.transform.position, location) > .2f && t < maxWalkTime)
        {
            t += Time.deltaTime;
            yield return null;
        }

        _entity.agent.isStopped = true;
        StartCoroutine(FaceTarget());

    }


}

