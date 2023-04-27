using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilAI;

[CreateAssetMenu(fileName = "FoodProximity_F", menuName = "ScriptObjects/Factors/FoodProximity", order = 0)]
public class FoodProximity_Factor : Factor
{
    public float maxDistance;

    public override float Evaluate()
    {
        float distance = Mathf.Infinity;

        foreach (Food food in blackboard.FoodDetected)
        {
            float newDistance = Vector3.Distance(food.transform.position, blackboard.transform.position);

            if (newDistance < distance)
            {
                distance = newDistance;
            }
        }

        float distancePercent = distance / maxDistance;
        distancePercent = Mathf.Clamp01(distancePercent);

        return 1 - distancePercent;
    }
}
