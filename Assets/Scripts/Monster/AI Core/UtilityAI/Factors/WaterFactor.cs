using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilAI
{
    [CreateAssetMenu(fileName = "Water_Factor", menuName = "ScriptObjects/Factors/Water", order = 0)]
    public class WaterFactor : Factor
    {
        public override float Evaluate()
        {
            float percent = blackboard.WaterMeter;

            float evaluatedValue = evaluationCurve.Evaluate(percent);

            return evaluatedValue;
        }
    }
}