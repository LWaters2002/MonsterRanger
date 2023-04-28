using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualSensor : Sensor
{
    public float passiveRange;
    public float activeRange;

    public LayerMask layer;

    public float visionTickRate;

    private HashSet<IDetectable> _detectablesInRange;

    public override void Init()
    {
        base.Init();
        _detectablesInRange = new HashSet<IDetectable>();
        InvokeRepeating("VisionTick", 0.0f, visionTickRate);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }

    protected override void DectableEnter(IDetectable detectable)
    {
        _detectablesInRange.Add(detectable);
    }

    protected override void DectableExit(IDetectable detectable)
    {
        _detectablesInRange.Add(detectable);
    }

    protected override void RemoveNulls()
    {
        _detectablesInRange.RemoveWhere(x => ((x as UnityEngine.Object) == null));
        base.RemoveNulls();
    }

    private void VisionTick()
    {
        RemoveNulls();
        foreach (IDetectable detectable in _detectablesInRange)
        {
            bool contains = _detectables.Contains(detectable);
            bool check = CheckLineOfSight(detectable);

            if (!contains && check) StartCoroutine(AddDetectable(detectable));
            if (contains && !check) StartCoroutine(RemoveDectectable(detectable));
        }
    }

    private bool CheckLineOfSight(IDetectable detectable)
    {
        Vector3 direction = (-transform.position + detectable.gameObject.transform.position).normalized;

        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, activeRange, layer))
        {
            bool exists = CheckTransformExistsInChildren(hit.collider.transform, detectable.gameObject.transform);

            Color color = (exists) ? Color.green : Color.blue;

            if (showDebug) Debug.DrawRay(transform.position, direction * passiveRange, color, visionTickRate);

            return exists;
        }

        return false;

    }

    private bool CheckTransformExistsInChildren(Transform t0, Transform t1)
    {
        foreach (Transform transf in t1)
        {
            if (transf == t0)
            {
                return true;
            }
        }

        return false;
    }

}
