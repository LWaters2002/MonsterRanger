using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    public float reactionTime;

    public System.Action<IDetectable> OnDetectEnter;
    public System.Action<IDetectable> OnDetectLeave;

    public List<IDetectable> _detectables { get; protected set; }

    public bool showDebug;

    public virtual void Init()
    {
        _detectables = new List<IDetectable>();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {

        IDetectable detectable = other.GetComponentInParent<IDetectable>();

        if (detectable == null) return;
        if (_detectables.Contains(detectable)) return;

        DectableEnter(detectable);
    }

    protected void OnTriggerExit(Collider other)
    {
        IDetectable detectable = other.GetComponentInParent<IDetectable>();
        if (!_detectables.Contains(detectable)) return;

        DectableExit(detectable);
    }

    protected virtual void DectableEnter(IDetectable detectable)
    {

    }

    protected virtual void DectableExit(IDetectable detectable)
    {
        StartCoroutine(RemoveDectectable(detectable));
    }

    protected IEnumerator AddDetectable(IDetectable detectable)
    {
        yield return new WaitForSeconds(reactionTime);
        _detectables.Add(detectable);
        OnDetectEnter?.Invoke(detectable);
    }

    protected IEnumerator RemoveDectectable(IDetectable detectable)
    {
        yield return new WaitForSeconds(reactionTime);
        OnDetectLeave?.Invoke(detectable);
        _detectables.Remove(detectable);
    }
}

