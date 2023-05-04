using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilAI
{
    [CreateAssetMenu(fileName = "WaterFactor_F", menuName = "ScriptObjects/Factors/Water", order = 0)]
    public class WaterFactor_F : Factor
    {
        public override float Evaluate()
        {
            float score = blackboard.GetWaterMeter();
            evaluationCurve.Evaluate(score);

            return 1 - score;
        }
    }
}
