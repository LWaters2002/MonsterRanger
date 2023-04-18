using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Footprint : Track
{
    public LUI.UI footprintMinigameUI;

    bool interactHeld;

    public override void StartedLooking(PlayerInteractor interactor)
    {
        base.StartedLooking(interactor);
        interactor.Player.controls.Gameplay.Interact.performed += ProcessInteractButton;
    }

    private void Update()
    {
        if (interactHeld)
        {
            LUI.UIHolder.AddElement(footprintMinigameUI);
            Destroy(gameObject);
        }
    }

    private void ProcessInteractButton(InputAction.CallbackContext ctx)
    {
        interactHeld = ctx.ReadValue<float>() > .5f;
    }

    public override void StoppedLooking(PlayerInteractor interactor)
    {
        base.StoppedLooking(interactor);
        interactor.Player.controls.Gameplay.Interact.performed -= ProcessInteractButton;
    }

    public override string ToString()
    {
        return base.ToString();
    }

}
