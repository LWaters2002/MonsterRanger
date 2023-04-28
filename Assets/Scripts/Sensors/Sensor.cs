using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    public float reactionTime;

    public System.Action<IDetectable> OnDetectEnter;
    public System.Action<IDetectable> OnDetectLeave;
    public System.Action RemovedNulls;

    public HashSet<IDetectable> _detectables { get; protected set; }

    public bool showDebug;

    public virtual void Init()
    {
        _detectables = new HashSet<IDetectable>();
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
        if (!_detectables.Contains(detectable))
        {
            _detectables.Add(detectable);
            detectable.onDestroy += RemoveNulls;
            OnDetectEnter?.Invoke(detectable);
        }
    }

    protected virtual void RemoveNulls()
    {
        _detectables.RemoveWhere(x => ((x as UnityEngine.Object) == null));
        RemovedNulls?.Invoke();
    }

    protected IEnumerator RemoveDectectable(IDetectable detectable)
    {
        yield return new WaitForSeconds(reactionTime);
        if (_detectables.Contains(detectable))
        {
            OnDetectLeave?.Invoke(detectable);
            _detectables.Remove(detectable);
        }
    }
}

