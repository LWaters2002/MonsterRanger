using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualSensor : Sensor
{
    protected override void SensorTick()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDetectable spottable))
        {
            detectables.Add(spottable);
            Invoke("UpdateBlackboard", responseTime);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IDetectable spottable))
        {
            detectables.Remove(spottable);
            Invoke("UpdateBlackboard", responseTime);
        }
    }
}
