using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDetectable
{
    public GameObject gameObject { get; }
}

public interface IInteractable
{
    public GameObject gameObject { get; }

    public void StartedLooking(PlayerInteractor interactor);
    public void StoppedLooking(PlayerInteractor interactor);
}

public interface IDamagable
{
    public GameObject gameObject { get; }

    public void TakeDamage(Damage damage);
}

public struct Damage 
{
    public float amount;
}