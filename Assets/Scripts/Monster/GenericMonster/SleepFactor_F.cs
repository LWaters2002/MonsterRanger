using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilAI
{
    [CreateAssetMenu(fileName = "SleepFactor_F", menuName = "ScriptObjects/Factors/Sleep", order = 0)]
    public class SleepFactor_F : Factor
    {
        public override float Evaluate()
        {
            float score = blackboard.GetSleepMeter();
            evaluationCurve.Evaluate(score);

            return 1 - score;
        }
    }
}
