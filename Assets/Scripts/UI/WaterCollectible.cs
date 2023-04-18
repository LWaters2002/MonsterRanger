using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCollectible : MonoBehaviour
{

    void OnTriggerStay(Collider other)
    {
        //TODO replace this is just for prototyping
        if (other.TryGetComponent(out Entity entity))
        {
            // ExampleBlackboard blackboard = (ExampleBlackboard)entity.blackboard;

            // if (!blackboard) return;

            // blackboard.water += 0.02f;
        }
    }
}
