using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilAI
{
    [CreateAssetMenu(fileName = "Food_Factor", menuName = "ScriptObjects/Factors/Food", order = 0)]
    public class FoodFactor : Factor
    {
        public override float Evaluate()
        {
            float percent = blackboard.hunger / blackboard.maxHunger;

            float evaluatedValue = evaluationCurve.Evaluate(percent);

            return evaluatedValue;
        }

    }
}