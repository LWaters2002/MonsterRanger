using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilAI
{
    [RequireComponent(typeof(Entity))]
    public class UtilityAI : MonoBehaviour
    {
        #region Variables
        public Entity entity { get; private set; }
        public Blackboard blackboard { get; private set; }

        public List<Action> actions;

        public float reactiveTickInterval;

        public Action currentAction { get; private set; }

        private List<Action> _longActions;
        private List<Action> _liveActions;

        private Stack<Action> _reactiveStack;
        private Stack<Action> _immediateStack;

        #endregion

        public void Init(Entity entity)
        {
            this.entity = entity;

            blackboard = GetComponent<Blackboard>();
            blackboard.Init();

            InitialiseAndSortActions();

            ExecuteOptimalAction();
        }

        private void InitialiseAndSortActions()
        {
            _liveActions = new List<Action>();
            _longActions = new List<Action>();
            _reactiveStack = new Stack<Action>();
            _immediateStack = new Stack<Action>();

            for (int i = 0; i < actions.Count; i++)
            {
                actions[i] = Instantiate(actions[i]);
                actions[i].Init(this, entity);

                if (actions[i].type == ActionType.goal)
                {
                    _longActions.Add(actions[i]);
                    continue;
                }

                _liveActions.Add(actions[i]);
            }

            InvokeRepeating("LiveActionTick", 0.0f, reactiveTickInterval);
        }

        #region Tick
        private void LiveActionTick()
        {
            foreach (Action action in _liveActions)
            {
                action.CalculateScore();
            }
        }

        private void Update()
        {
            currentAction?.Tick(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            currentAction?.FixedTick(Time.fixedDeltaTime);
        }

        #endregion

        #region Action Methods
        public void ExecuteOptimalAction()
        {
            if (_immediateStack.Count > 0)
            {
                SetCurrentAction(_immediateStack.Pop());
                return;
            }

            if (_reactiveStack.Count > 0)
            {
                SetCurrentAction(_immediateStack.Pop());
                return;
            }

            SetCurrentAction(GetBestLongAction());
        }

        private void SetCurrentAction(Action actionToSet)
        {
            if (!actionToSet) return;
            if (actionToSet == currentAction) return;

            if (currentAction) currentAction.OnComplete -= ActionFinished;

            currentAction = actionToSet;

            currentAction.OnComplete += ActionFinished;
            currentAction.Entry();
        }

        private Action GetBestLongAction()
        {
            Action actionWithHighestScore = null;
            float score = 0f;

            foreach (Action action in _longActions)
            {
                float actionScore = action.CalculateScore();

                if (actionScore < score) continue;

                actionWithHighestScore = action;
                score = actionScore;
            }

            return actionWithHighestScore;
        }

        public void AddReactiveAction(Action action)
        {
            if (_reactiveStack.Contains(action)) return;
            _reactiveStack.Push(action);
            ExecuteOptimalAction();
        }

        public void AddImmediateAction(Action action)
        {
            if (_immediateStack.Contains(action)) return;
            _immediateStack.Push(action);
            ExecuteOptimalAction();
        }

        private void ActionFinished()
        {
            ExecuteOptimalAction();
        }

        #endregion
    }
}
