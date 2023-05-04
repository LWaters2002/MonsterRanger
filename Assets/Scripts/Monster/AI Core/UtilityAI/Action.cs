using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilAI
{
    public abstract class Action : ScriptableObject
    {
        [HideInInspector]
        public float score;
        public float scoreMultiplier = 1.0f;

        public float baseScore;

        public ActionType type;
        public float deadThreshold;
        public float liveThreshold;

        protected Entity entity;

        public Factor[] factors;
        public System.Action OnComplete;

        protected UtilityAI utilityAI;

        public virtual void Init(UtilityAI utilityAI, Entity entity)
        {
            this.utilityAI = utilityAI;
            this.entity = entity;

            for (int i = 0; i < factors.Length; i++)
            {
                factors[i] = Instantiate(factors[i]);
                factors[i].Init(entity);
            }
        }

        public virtual float CalculateScore()
        {
            float sum = 0;
            float combinedWeight = 0;

            foreach (Factor f in factors)
            {
                combinedWeight += f.weight;
                sum += f.Evaluate();
            }

            //sum /= combinedWeight;
            score = sum / factors.Length;

            if (score > liveThreshold)
            {
                if (type == ActionType.reactive) utilityAI.AddReactiveAction(this);
                if (type == ActionType.immeidate) utilityAI.AddImmediateAction(this);
            }

            return score * scoreMultiplier;
        }

        public abstract void Entry();
        public abstract void Tick(float deltaTime);
        public abstract void FixedTick(float fixedDeltaTime);

        public virtual void Complete()
        {
            OnComplete?.Invoke();
        }
    }
}

public enum ActionType
{
    goal,
    reactive,
    immeidate
}

public class Timer
{
    public System.Action TimerComplete;

    private float _time = 0;
    private float _length;

    public Timer(float length)
    {
        _length = length;
    }

    public void Tick(float deltaTime)
    {
        _time += Time.deltaTime;

        if (_time > _length)
        {
            TimerComplete?.Invoke();
        }
    }
}