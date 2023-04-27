using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SensorManager : MonoBehaviour
{
    private HashSet<Sensor> _sensors;
    private List<IDetectable> _detectables;
    private List<IDetectable> _rememberedDetectables;

    public float memoryDuration;

    public Action<IDetectable> DetectableAdded;
    public Action<IDetectable> DetectableRemoved;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public void Init()
    {
        _sensors = GetComponentsInChildren<Sensor>().ToHashSet();

        foreach (Sensor sensor in _sensors)
        {
            sensor.Init();

            sensor.OnDetectEnter += SensorEnter;
            sensor.OnDetectLeave += SensorLeave;
        }
    }

    private void SensorLeave(IDetectable detectable)
    {

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
        DetectableAdded?.Invoke(detectable);
        _detectables.Add(detectable);
    }

}
