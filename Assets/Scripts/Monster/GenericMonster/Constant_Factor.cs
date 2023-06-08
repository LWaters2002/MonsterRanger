using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilAI
{
    [CreateAssetMenu(fileName = "Constant_F", menuName = "ScriptObjects/Factors/Constant", order = 0)]
    public class Constant_Factor : Factor
    {
        public float constantScore;

        public override float Evaluate()
        {
            return constantScore;
        }
    }
}