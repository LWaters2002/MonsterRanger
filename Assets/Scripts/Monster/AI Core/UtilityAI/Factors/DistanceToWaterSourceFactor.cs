using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UtilAI
{
    [CreateAssetMenu(fileName = "DistanceToWater_Factor", menuName = "ScriptObjects/Factors/DistanceToWater", order = 0)]
    public class DistanceToWaterSourceFactor : Factor
    {
        public override float Evaluate()
        {
            Vector3 closestWaterArea = blackboard.WaterAreas.GetClosestArea(blackboard.transform.position);

            float percent = Vector3.Distance(blackboard.transform.position, closestWaterArea) / blackboard.maxTravelDistance;
            float evaluatedValue = evaluationCurve.Evaluate(percent);

            return evaluatedValue;
        }
    }
}