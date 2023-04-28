using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumeFood_Attack : Attack_Gallant
{
    public FoodConsumeProjectile projectilePrefab;
    private FoodConsumeProjectile _projectile;

    public Transform headTransform;

    private bool _turn;

    public override void Attack(int attackStep)
    {
        base.Attack(attackStep);

        switch (attackStep)
        {
            case 0:

                SpawnProjectile();
                StartCoroutine(TurnToTarget());
                break;
            case 1:
                ReturnProjectile();
                _turn = false;
                break;
            case 2:
                Eat();
                break;
            case 3:
                AttackComplete();
                break;
        }
    }

    IEnumerator TurnToTarget()
    {
        _turn = true;
        Quaternion targetRotation = Quaternion.LookRotation(_entity.blackboard.Target.transform.position - transform.position);

        while (_turn)
        {
            _entity.transform.rotation = Quaternion.Lerp(_entity.transform.rotation, targetRotation, Time.deltaTime * 4f);
            yield return null;
        }
    }

    private void SpawnProjectile()
    {
        _projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        _projectile.Init(0f, _entity.blackboard.Target.transform.position, _entity);
    }

    private void ReturnProjectile()
    {
        _projectile.SetTarget(headTransform.position, 2f);
    }

    private void Eat()
    {
        Destroy(_projectile.gameObject);
    }



}
