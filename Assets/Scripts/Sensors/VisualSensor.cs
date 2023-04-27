using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualSensor : Sensor
{
    public float passiveRange;
    public float activeRange;

    public LayerMask layer;

    protected override void DectableEnter(IDetectable detectable)
    {
        Vector3 direction = (transform.position - detectable.gameObject.transform.position).normalized;

        //Checks for Line Of Sight
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, passiveRange, layer))
        {
            if (hit.collider.gameObject == detectable.gameObject)
            {
                StartCoroutine(AddDetectable(detectable));
            }
        }
    }

    private void Update()
    {
        foreach (IDetectable detectable in _detectables)
        {
            Vector3 direction = (transform.position - detectable.gameObject.transform.position).normalized;

            //Checks for Line Of Sight
            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, activeRange, layer))
            {
                if (hit.collider.gameObject == detectable.gameObject)
                {
                    continue;
                }
            }

            StartCoroutine(RemoveDectectable(detectable));
        }


    }
}
