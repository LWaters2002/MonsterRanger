using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilAI;
using System.Linq;

namespace UtilActions
{
    [CreateAssetMenu(fileName = "FoodReactor_A", menuName = "ScriptObjects/Actions/FoodReactor", order = 0)]
    public class FoodSourceReaction : Action
    {
        private Food _targetFood;

        public Action ConsumeAction;

        bool consuming = false;

        public override void Entry()
        {
            _targetFood = entity.blackboard.FoodDetected.OrderBy(x => Vector3.Distance(x.transform.position, entity.transform.position)).First();

            if (!_targetFood) { Complete(); return; }

            entity.agent.SetDestination(_targetFood.transform.position);
        }

        public override void Tick(float deltaTime)
        {
            if (consuming) return;

            float distanceToFood = Vector3.Distance(entity.transform.position, _targetFood.transform.position);

            if (distanceToFood < entity.blackboard.foodConsumeRange)
            {
                utilityAI.AddImmediateAction(ConsumeAction);
                consuming = true;
            }
        }

        public override void FixedTick(float fixedDeltaTime)
        {
        }
    }

}