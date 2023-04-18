using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Events;
using System;

public class Track : MonoBehaviour, IInteractable
{
    protected Material _decalMaterial;
    protected PlayerInteractor _interactor;

    public UnityEvent OnLookAt;
    public UnityEvent OnLookLeave;

    void Awake()
    {
        DecalProjector projector = GetComponentInChildren<DecalProjector>();

        if (projector)
        {
            _decalMaterial = new Material(projector.material);
            projector.material = _decalMaterial;

            _decalMaterial.SetFloat("_Highlight", 0f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.transform.parent) return;

        if (other.transform.parent.TryGetComponent(out PlayerCharacter player))
        {
            _decalMaterial?.SetFloat("_Highlight", 1f);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.transform.parent) return;

        if (other.transform.parent.TryGetComponent(out PlayerCharacter player))
        {
            _decalMaterial?.SetFloat("_Highlight", 0f);
        }
    }

    public virtual void StartedLooking(PlayerInteractor interactor)
    {
        _interactor = interactor;
        OnLookAt?.Invoke();
    }

    public virtual void StoppedLooking(PlayerInteractor interactor)
    {
        if (interactor != _interactor) return;
        OnLookLeave?.Invoke();
        _interactor = null;
    }
}
