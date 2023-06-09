using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleAttack_Gallant : Attack_Gallant
{
    public Animator animator;

    public EnemyProjectile projectilePrefab;
    private EnemyProjectile _projectile;

    public Transform projectileSpawn;

    public float turnAmount = 2f;
    public float launchForce = 60f;

    int spawnIndex;

    public override void Attack(int attackStep)
    {
        base.Attack(attackStep);

        _entity.agent.isStopped = true;

        IEnumerator turning = Turn();

        switch (attackStep)
        {
            case 0:
                CreateOrbs();
                StartCoroutine(turning);
                break;
            case 1:
                ShootOrb();
                break;
            case 2:
                ShootOrb();
                break;
            case 3:
                StopCoroutine(turning);
                ShootOrb();
                AttackComplete();
                break;
        }
    }



    private IEnumerator Turn()
    {
        while (true)
        {
            _entity.TurnToTargetTicked(turnAmount);
            yield return null;
        }
    }


    private void CreateOrbs()
    {
        animator.Play("charge");
    }

    private void ShootOrb()
    {
        Vector3 targetPos = _entity.blackboard.Target.transform.position;

        _projectile = Instantiate(projectilePrefab, projectileSpawn.position, Quaternion.identity);
        _projectile.Init(60f, targetPos, _entity);
        _projectile.SetState(ProjectileState.Physics);
        _projectile.LaunchAtTarget(_entity.blackboard.Target.transform.position, launchForce);
    }
}
