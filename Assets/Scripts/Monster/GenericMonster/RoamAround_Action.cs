using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilAI;
using System.Linq;
using UnityEngine.AI;

namespace UtilActions
{
    [CreateAssetMenu(fileName = "RoamAround_A", menuName = "ScriptObjects/Actions/RoamAround", order = 0)]
    public class RoamAround_Action : Action
    {
        private PlayerCharacter playerCharacter;
        public LayerMask layer;
        public float roamRange;

        private Vector3 targetPosition;

        public override void Init(UtilityAI utilityAI, Entity entity)
        {
            base.Init(utilityAI, entity);
        }

        public override void Entry()
        {
            MoveToNewPosition();
        }

        private void MoveToNewPosition()
        {
            entity.agent.isStopped = false;

            Vector3 position = entity.blackboard.RoamArea.transform.position + (Random.Range(-roamRange, roamRange) * Vector3.right) + (Random.Range(-roamRange, roamRange) * Vector3.forward);

            NavMeshHit hit;
            NavMesh.SamplePosition(position, out hit, 20f, layer);

            targetPosition = hit.position;

            entity.agent.SetDestination(targetPosition);
        }

        public override void Tick(float deltaTime)
        {
            float distance = Vector3.Distance(entity.transform.position, targetPosition);

            if (distance < 2f) MoveToNewPosition();
        }

        public override void FixedTick(float fixedDeltaTime)
        {
        }

        public override void Complete()
        {
            entity.agent.isStopped = true;

            base.Complete();
        }
    }

}