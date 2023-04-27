using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilAI
{
    public abstract class SequencedAction : Action
    {
        public Action[] subActions;
        private int _actionIndex = -1;

        public override void Init(UtilityAI utilityAI, Entity entity)
        {
            base.Init(utilityAI, entity);

            for (int i = 0; i < subActions.Length; i++)
            {
                subActions[i] = Instantiate(subActions[i]);
                subActions[i].Init(utilityAI, entity);
            }
        }

        public override void Entry()
        {
            _actionIndex = -1;
            NextSubAction();
        }

        public override void Tick(float deltaTime)
        {
            subActions[_actionIndex].Tick(deltaTime);
        }

        public override void FixedTick(float fixedDeltaTime)
        {
            subActions[_actionIndex].FixedTick(fixedDeltaTime);
        }

        protected void NextSubAction()
        {
            if (_actionIndex != -1) subActions[_actionIndex].OnComplete -= NextSubAction;

            _actionIndex++;

            if (_actionIndex == subActions.Length) { Complete(); return; }

            subActions[_actionIndex].OnComplete += NextSubAction;
            subActions[_actionIndex].Entry();
        }

        public override void Complete()
        {
            base.Complete();
        }
    }
}
