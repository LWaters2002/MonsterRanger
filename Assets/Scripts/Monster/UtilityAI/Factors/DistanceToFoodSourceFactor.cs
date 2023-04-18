using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilAI
{
    [CreateAssetMenu(fileName = "DistanceToFood_Factor", menuName = "ScriptObjects/Factors/DistanceToFood", order = 0)]
    public class DistanceToFoodSourceFactor : Factor
    {
        public override float Evaluate()
        {
            Vector3 closestFoodArea = blackboard.GetClosestFood();
            
            float percent =  Vector3.Distance(blackboard.transform.position, closestFoodArea)/blackboard.maxTravelDistance;
            float evaluatedValue = evaluationCurve.Evaluate(percent);
            
            return evaluatedValue;
        }

    }
}