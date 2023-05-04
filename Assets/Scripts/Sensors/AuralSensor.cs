using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuralSensor : Sensor
{
    public float audioTick = .2f;

    private HashSet<AudioSource> _audioSources;

    public override void Init()
    {
        base.Init();
        _audioSources = new HashSet<AudioSource>();

        InvokeRepeating("AudioTick", 0f, audioTick);
    }

    private void AudioTick()
    {
        //
    }

    protected override void DectableEnter(IDetectable detectable)
    {
        base.DectableEnter(detectable);
        StartCoroutine(AddDetectable(detectable));
    }
}
