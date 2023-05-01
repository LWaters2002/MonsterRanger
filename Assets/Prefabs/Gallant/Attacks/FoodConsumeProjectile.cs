using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodConsumeProjectile : EnemyProjectile
{
    public Transform childTransform;
    public float pullForce = 35f;

    public System.Action<Food> FoodHit;

    private void OnTriggerEnter(Collider other)
    {
        Food food = other.GetComponentInParent<Food>();

        if (food)
        {
            OnHit?.Invoke();
            FoodHit?.Invoke(food);
            food.transform.SetParent(childTransform);
            return;
        }

        PlayerCharacter player = other.GetComponentInParent<PlayerCharacter>();

        if (player)
        {
            Vector3 dir = -_rigidbody.velocity.normalized + Vector3.one * .2f;
            player.rb.AddForce(dir * pullForce, ForceMode.Impulse);
            return;
        }
    }


}
