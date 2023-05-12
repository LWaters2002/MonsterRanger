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

    public int logbookInformationID = -1;

    public Transform locationReveal;
    public PointerArrow pointerArrowPrefab;

    [Header("Events")]
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

    public virtual void StartedLooking(PlayerInteractor interactor)
    {
        _interactor = interactor;
        _decalMaterial?.SetFloat("_Highlight", 1f);
        OnLookAt?.Invoke();
    }

    public virtual void StoppedLooking(PlayerInteractor interactor)
    {
        if (interactor != _interactor) return;
        OnLookLeave?.Invoke();
        _decalMaterial?.SetFloat("_Highlight", 0f);
        _interactor = null;
    }

    public virtual void Interact(PlayerInteractor interactor)
    {
        interactor.Player.informationLog.RevealInformation(logbookInformationID);
        CreatePointerArrow();
        Destroy(gameObject);
    }

    protected void CreatePointerArrow()
    {
        if (!locationReveal) return;

        PointerArrow pointerArrow = Instantiate(pointerArrowPrefab, transform.position, Quaternion.identity);
        pointerArrow.Init(locationReveal.position);
    }
}
