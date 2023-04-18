using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace UtilAI
{
    [RequireComponent(typeof(Entity))]
    public class UtilityAI : MonoBehaviour
    {
        public Entity entity { get; private set; }
        public Blackboard blackboard { get; private set; }

        public List<Action> actions;
        public Action currentAction { get; private set; }

        public void Init(Entity entity)
        {
            this.entity = entity;

            blackboard = GetComponent<Blackboard>();
            blackboard.Init();

            for (int i = 0; i < actions.Count; i++)
            {
                actions[i] = Instantiate(actions[i]);
                actions[i].Init(entity);
            }

            ExecuteBestAction();
        }

        private void Update()
        {
            currentAction?.Tick(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            currentAction?.FixedTick(Time.fixedDeltaTime);
        }

        public void ExecuteBestAction()
        {
            if (currentAction) currentAction.OnComplete -= ActionFinished;

            actions.ForEach(x => x.CalculateScore());
            currentAction = actions.OrderByDescending(x => x.score).ToArray()[0];

            currentAction.OnComplete += ActionFinished;
            currentAction.Entry();
        }

        private void ActionFinished()
        {
            ExecuteBestAction();
        }

    }
}
