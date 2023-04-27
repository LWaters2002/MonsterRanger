using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuralSensor : Sensor
{

    private HashSet<AudioSource> _audioSources;

    protected override void OnTriggerEnter(Collider other)
    {

        AudioSource source = other.GetComponentInParent<AudioSource>();
        if (!source) return;

        _audioSources.Add(source);
    }

    private void Update()
    {
        foreach (AudioSource source in _audioSources)
        {
            if (source.isPlaying)
            {
                
            }
        }
    }

    protected override void DectableEnter(IDetectable detectable)
    {
        detectable.gameObject.GetComponentInParent<AudioSource>();
    }



}
