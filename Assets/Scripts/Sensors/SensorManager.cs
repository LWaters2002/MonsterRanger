using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SensorManager : MonoBehaviour
{
    public HashSet<Sensor> _sensors;

    public HashSet<IDetectable> _detectables { get; private set; }
    private HashSet<IDetectable> _forgottenDetectables;

    public float memoryDuration;

    public Action<IDetectable> DetectableAdded;
    public Action<IDetectable> DetectableRemoved;

    public void Init()
    {
        _sensors = GetComponentsInChildren<Sensor>().ToHashSet();

        _detectables = new HashSet<IDetectable>();
        _forgottenDetectables = new HashSet<IDetectable>();

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

        if (!_forgottenDetectables.Contains(detectable)) return;
        StartCoroutine(ForgetDetectable(detectable));
    }

    private IEnumerator ForgetDetectable(IDetectable detectable)
    {
        _forgottenDetectables.Add(detectable);

        yield return new WaitForSeconds(memoryDuration);

        if (_forgottenDetectables.Contains(detectable))
        {
            DetectableRemoved?.Invoke(detectable);
            _detectables.Remove(detectable);
        }
    }

    private void SensorEnter(IDetectable detectable)
    {
        if (_detectables.Contains(detectable)) return;
        if (_forgottenDetectables.Contains(detectable)) _forgottenDetectables.Remove(detectable);

        DetectableAdded?.Invoke(detectable);
        _detectables.Add(detectable);
    }

}
