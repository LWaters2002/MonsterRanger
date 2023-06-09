using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipToFight : MonoBehaviour, IInteractable
{
    public Transform point;

    public void Interact(PlayerInteractor interactor)
    {
        interactor.Player.rb.MovePosition(point.position);
    }

    public void StartedLooking(PlayerInteractor interactor)
    {
    }

    public void StoppedLooking(PlayerInteractor interactor)
    {
    }
}
