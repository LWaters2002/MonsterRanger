using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodCollectible : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Entity entity))
        {
            // ExampleBlackboard blackboard = (ExampleBlackboard)entity.blackboard;

            // if (!blackboard) return;

            // blackboard.food = 1;
            // Destroy(gameObject);
        }
    }
}
