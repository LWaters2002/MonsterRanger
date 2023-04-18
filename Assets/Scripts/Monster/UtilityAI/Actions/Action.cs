using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilAI
{
    public abstract class Action : ScriptableObject
    {
        [HideInInspector]
        public float score;
        public float baseScore;

        protected Entity entity;

        public Factor[] factors;
        public System.Action OnComplete;

        public virtual void Init(Entity entity)
        {
            this.entity = entity;

            for (int i = 0; i < factors.Length; i++)
            {
                factors[i] = Instantiate(factors[i]);
                factors[i].Init(entity);
            }
        }

        public virtual void CalculateScore()
        {

            float sum = 0;
            float combinedWeight = 1;

            foreach (Factor f in factors) // Come back to this
            {
                combinedWeight += f.weight;
                sum += f.Evaluate() * f.weight;
            }

            sum /= combinedWeight;
            score = sum / factors.Length;
        }

        public abstract void Entry();
        public abstract void Tick(float deltaTime);
        public abstract void FixedTick(float fixedDeltaTime);

        protected virtual void Complete()
        {
            OnComplete?.Invoke();
        }


    }
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