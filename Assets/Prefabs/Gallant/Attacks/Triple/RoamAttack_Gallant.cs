using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoamAttack_Gallant : Attack_Gallant
{

    public float maxWalkTime = 1f;
    public float speed = 8;
    public float acceptanceRadius;

    public override void StartAttack()
    {
        Transform targetT = _entity.blackboard.Target.transform;

        Vector3 direction = (targetT.position - transform.position).normalized;
        Vector3 crossPro = Vector3.Cross(direction, Vector3.up);

        Vector3 position = targetT.position + (Random.Range(-6, 6) * crossPro) + (Random.Range(2, 8) * direction);

        NavMeshHit hit;
        NavMesh.SamplePosition(position, out hit, 5f, ~0);

        Vector3 targetPosition = hit.position;

        StartCoroutine(_entity.WalkToPosition(AttackComplete, targetPosition, acceptanceRadius, speed, maxWalkTime));
    }

}
