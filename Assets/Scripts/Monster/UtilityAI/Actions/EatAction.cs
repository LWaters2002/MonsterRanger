using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilAI
{

    [CreateAssetMenu(fileName = "Eat", menuName = "ScriptObjects/Actions/Eat", order = 0)]
    public class EatAction : Action
    {

        private int _consumables = 0;

        public override void Entry()
        {
            Vector3 foodArea = entity.blackboard.GetClosestFood();
            entity.GoToLocation(foodArea);

            entity.ArrivedAtLocation += Arrived;

            _consumables = 0;
        }

        private void Arrived()
        {
            entity.ArrivedAtLocation -= Arrived;
            Collider[] colliders = Physics.OverlapSphere(entity.transform.position, 100f);

            bool foodFound = false;

            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.TryGetComponent(out FoodSource source))
                {
                    foodFound = true;
                    _consumables += 1;
                    source.OnConsume += FoodConsumed;
                }
            }

            if (foodFound)
            {
                entity.animator.SetBool("ActionPlaying", true);
                entity.animator.CrossFade("Eat", .2f);
            }
        }

        private void FoodConsumed()
        {
            _consumables--;

            if (_consumables <= 0)
            {
                entity.animator.SetBool("ActionPlaying", false);
                Complete();
            }
        }

        public override void FixedTick(float fixedDeltaTime)
        {

        }

        public override void Tick(float deltaTime)
        {

        }
    }
}