using UnityEngine;
using UnityEditor;

namespace UtilAI
{
    public abstract class Factor : ScriptableObject
    {
        public AnimationCurve evaluationCurve;
        public float weight = 1;

        public abstract float Evaluate();

        protected EntityBlackboard blackboard;

        public virtual void Init(Entity entity)
        {
            blackboard = entity.blackboard;
        }   
    }
}