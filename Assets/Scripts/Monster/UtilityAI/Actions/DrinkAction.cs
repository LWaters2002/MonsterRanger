using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilAI
{

    [CreateAssetMenu(fileName = "Drink", menuName = "ScriptObjects/Actions/Drink", order = 0)]
    public class DrinkAction : Action
    {
        Timer drinkTimer;

        public override void Entry()
        {
            Vector3 waterArea = entity.blackboard.GetClosestWater();
            entity.GoToLocation(waterArea);

            entity.ArrivedAtLocation += ArrivedAtWater;
        }

        private void ArrivedAtWater()
        {
            entity.ArrivedAtLocation -= ArrivedAtWater;
            entity.animator.SetBool("ActionPlaying", true);
            entity.animator?.CrossFade("Drink", .2f);

            drinkTimer = new Timer(2f);
            drinkTimer.TimerComplete += Complete;
        }

        public override void FixedTick(float fixedDeltaTime)
        {
        }

        public override void Tick(float deltaTime)
        {
            drinkTimer?.Tick(deltaTime);
        }

        protected override void Complete()
        {
            base.Complete();

            entity.blackboard.water = entity.blackboard.maxWater;
            entity.animator.SetBool("ActionPlaying", false);

            drinkTimer = null;
        }

    }
}