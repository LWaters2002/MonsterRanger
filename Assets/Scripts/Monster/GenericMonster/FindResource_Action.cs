using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilAI
{
    [CreateAssetMenu(fileName = "FindResource_A", menuName = "ScriptObjects/Actions/FindResource", order = 0)]
    public class FindResource_Action : Action
    {

        public AreaType areaType;

        public override void Entry()
        {
            entity.agent.isStopped = false;

            Vector3 location = entity.blackboard.GetClosestArea(areaType);
            entity.agent.SetDestination(location);
        }

        public override void FixedTick(float fixedDeltaTime) { }

        public override void Tick(float deltaTime)
        {
            float meter = 0.0f;
            switch (areaType)
            {
                case AreaType.Water:
                    meter = entity.blackboard.GetWaterMeter();
                    break;
                case AreaType.Food:
                    meter = entity.blackboard.GetWaterMeter();
                    break;
                case AreaType.Sleep:
                    meter = entity.blackboard.GetSleepMeter();
                    break;
            }

            if (meter > .7f) { Complete(); }
        }

    }
}