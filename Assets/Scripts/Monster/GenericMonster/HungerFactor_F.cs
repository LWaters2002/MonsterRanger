using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilAI
{
    public class HungerFactor_F : Factor
    {
        public override float Evaluate()
        {
            float score = blackboard.GetFoodMeter();
            evaluationCurve.Evaluate(score);
            
            return 1 - score;
        }
    }
}
