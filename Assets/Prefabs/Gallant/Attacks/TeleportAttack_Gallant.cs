using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TeleportAttack_Gallant : Attack_Gallant
{
    public override void Attack(int attackStep)
    {
        base.Attack(attackStep);

        switch (attackStep)
        {
            case 0:
                Transform target = _entity.blackboard.Target.transform;
                Vector3 inFrontOfPlayer = target.forward * 5f + target.position;

                NavMeshHit hit;
                if (NavMesh.SamplePosition(inFrontOfPlayer, out hit, 5f, NavMesh.AllAreas))
                {
                    transform.position = hit.position;
                    _entity.attackController.SelectAttackCombat("TailSwipe");
                }

                break;
        }
    }

    public override void Init(Entity entity)
    {
        base.Init(entity);
    }

    public override void StartAttack()
    {
        base.StartAttack();
    }

    public override void StopAttack()
    {
        base.StopAttack();
    }

}
