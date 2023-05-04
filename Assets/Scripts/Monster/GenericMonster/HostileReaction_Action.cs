using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilAI;
using System.Linq;

namespace UtilActions
{
    [CreateAssetMenu(fileName = "HostileReaction_A", menuName = "ScriptObjects/Actions/HostileReactor", order = 0)]
    public class HostileReaction_Action : Action
    {
        private PlayerCharacter playerCharacter;

        public override void Init(UtilityAI utilityAI, Entity entity)
        {
            base.Init(utilityAI, entity);
        }

        public override void Entry()
        {
            playerCharacter = entity.blackboard.GetDetected<PlayerCharacter>().First();

            if (!playerCharacter) Complete();

            entity.agent.isStopped = true;

            entity.blackboard.Target = playerCharacter.gameObject;

            entity.attackController.ChooseAttack();
        }

        public override void Tick(float deltaTime)
        {
            playerCharacter = entity.blackboard.GetDetected<PlayerCharacter>().First();

            if (!playerCharacter) Complete();
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