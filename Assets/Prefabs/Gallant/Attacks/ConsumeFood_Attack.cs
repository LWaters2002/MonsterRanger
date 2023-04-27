using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumeFood_Attack : Attack_Gallant
{
    public FoodConsumeProjectile projectilePrefab;
    private FoodConsumeProjectile _projectile;

    public Transform headTransform;

    public override void Attack(int attackStep)
    {
        base.Attack(attackStep);

        switch (attackStep)
        {
            case 0:
                SpawnProjectile();
                break;
            case 1:
                ReturnProjectile();
                break;
            case 2:
                Eat();
                break;
        }
    }

    private void SpawnProjectile()
    {
        _projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        _projectile.Init(0f, _entity.Target.transform.position, _entity);
    }

    private void ReturnProjectile()
    {
        _projectile.SetTarget(headTransform.position, 2f);
    }

    private void Eat()
    {
        Destroy(_projectile);
    }



}
