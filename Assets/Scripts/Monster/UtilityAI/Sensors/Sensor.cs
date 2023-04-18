using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using UtilAI;


public abstract class Sensor : MonoBehaviour
{
    public float responseTime;
    public float sensorTick;

    private EntityBlackboard blackboard;

    public HashSet<IDetectable> detectables { get; private set; }

    public void Init(EntityBlackboard blackboard)
    {
        this.blackboard = blackboard;

        detectables = new();
        InvokeRepeating("SensorTick", 0f, sensorTick);
    }

    protected abstract void SensorTick();

    protected virtual void UpdateBlackboard()
    {
        blackboard.UpdateDectatables();
    }

}
