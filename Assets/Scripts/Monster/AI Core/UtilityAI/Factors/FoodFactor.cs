using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilAI
{
    [CreateAssetMenu(fileName = "Food Factor", menuName = "ScriptObjects/Factors/Food", order = 0)]
    public class FoodFactor : Factor
    {
        public override float Evaluate()
        {
            float percent = blackboard.GetFoodMeter();

            float evaluatedValue = evaluationCurve.Evaluate(percent);

            return 1 - evaluatedValue;
        }
    }
}