using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SensorManager : MonoBehaviour
{
    public HashSet<Sensor> _sensors;

    public HashSet<IDetectable> _detectables { get; private set; }

    private HashSet<IDetectable> _rememberedDetectables;

    public float memoryDuration;

    public Action<IDetectable> DetectableAdded;
    public Action<IDetectable> DetectableRemoved;

    public void Init()
    {
        _sensors = GetComponentsInChildren<Sensor>().ToHashSet();

        _detectables = new HashSet<IDetectable>();
        _rememberedDetectables = new HashSet<IDetectable>();

        foreach (Sensor sensor in _sensors)
        {
            sensor.Init();

            sensor.OnDetectEnter += SensorEnter;
            sensor.OnDetectLeave += SensorLeave;
            sensor.RemovedNulls += RemoveNulls;
        }
    }

    public void RemoveNulls()
    {
        _detectables.RemoveWhere(x => ((x as UnityEngine.Object) == null));
    }

    private void SensorLeave(IDetectable detectable)
    {
        if (!_detectables.Contains(detectable)) return;

        // Check if other sensors contain detectable
        foreach (Sensor sensor in _sensors)
        {
            if (!sensor._detectables.Contains(detectable)) continue;

            return;
        }

        DetectableRemoved?.Invoke(detectable);
        _detectables.Remove(detectable);
    }

    private void SensorEnter(IDetectable detectable)
    {
        if (_detectables.Contains(detectable)) return;

        DetectableAdded?.Invoke(detectable);
        _detectables.Add(detectable);
    }

}
