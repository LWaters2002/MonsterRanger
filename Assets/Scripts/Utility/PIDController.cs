using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PIDController
{
    private PIDSettings _settings;

    public Vector3 errorLast;
    public Vector3 valueLast;

    public Vector3 integrationStored;

    public PIDController(ref PIDSettings settings)
    {
        _settings = settings;
    }

    public Vector3 Update(float deltaTime, Vector3 currentValue, Vector3 targetValue)
    {
        Vector3 error = targetValue - currentValue;

        //Calculate Proportionals
        Vector3 P = _settings.proportionGain * error;

        Vector3 errorRateOfChange = (error - errorLast) / deltaTime;
        errorLast = error;

        Vector3 D = _settings.derivativeGain * errorRateOfChange;

        integrationStored = integrationStored + (error * deltaTime);

        Vector3 I = _settings.integralGain * integrationStored;
        return P + I + D;
    }
}

[System.Serializable]
public struct PIDSettings
{
    public float proportionGain; // Acts like a spring
    public float integralGain;
    public float derivativeGain; // Acts like a dampener

    public float integrationSaturation;
}