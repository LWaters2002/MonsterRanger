using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodConsumeProjectile : EnemyProjectile
{
    public Transform childTransform;

    public System.Action<Food> FoodHit;

    private void OnTriggerEnter(Collider other)
    {
        Food food = other.GetComponentInParent<Food>();

        if (food)
        {
            OnHit?.Invoke();
            FoodHit?.Invoke(food);
            food.transform.SetParent(childTransform);
        }
    }


}
