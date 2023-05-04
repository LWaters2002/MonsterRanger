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

        public override void Init(UtilityAI utilityAI, Entity entity)
        {
            base.Init(utilityAI, entity);
            ConsumeAction = Instantiate(ConsumeAction);
            ConsumeAction.Init(utilityAI, entity);
        }

        public override void Entry()
        {
            ConsumeAction.OnComplete += SetTargetFood;

            List<Food> foodList = entity.blackboard.GetDetected<Food>();

            if (foodList.Count == 0) { Complete(); return; }

            _targetFood = foodList.
            OrderBy(x => Vector3.Distance(x.transform.position, entity.transform.position)).
            First();

            if (!_targetFood) { Complete(); return; }

            entity.agent.SetDestination(_targetFood.transform.position);
        }

        private void SetTargetFood()
        {
            consuming = false;

            List<Food> foodList = entity.blackboard.GetDetected<Food>();

            if (foodList.Count == 0) { Complete(); return; }

            _targetFood = foodList.
            OrderBy(x => Vector3.Distance(x.transform.position, entity.transform.position)).
            First();

            entity.agent.SetDestination(_targetFood.transform.position);
            entity.agent.isStopped = false;
        }

        private void CheckFood()
        {
            if (!_targetFood) { SetTargetFood(); return; }

            float distanceToFood = Vector3.Distance(entity.transform.position, _targetFood.transform.position);

            if (distanceToFood < entity.blackboard.foodConsumeRange)
            {
                entity.blackboard.Target = _targetFood.gameObject;
                utilityAI.AddImmediateAction(ConsumeAction);
                consuming = true;
            }
        }

        public override void Tick(float deltaTime)
        {
            if (consuming) return;

            CheckFood();
        }

        public override void FixedTick(float fixedDeltaTime)
        {
        }
    }

}