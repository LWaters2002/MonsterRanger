using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Interact Settings")]
    public float range;
    public LayerMask mask;
    public bool showDebug;

    public PlayerCharacter Player { get; private set; }
    private IInteractable _interactable;

    public void Init(PlayerCharacter player)
    {
        Player = player;

        Player.controls.Gameplay.Interact.performed += InteractWith;
    }

    private void InteractWith(InputAction.CallbackContext context)
    {
        if (_interactable != null) _interactable.Interact(this);
    }

    private void Update()
    {
        InteractRaycast();
    }

    private void InteractRaycast()
    {
        if (!Player) return;

        if (Physics.Raycast(Player.orientation.position, Player.orientation.forward, out RaycastHit hit, range, mask))
        {


            IInteractable interactable = hit.collider.gameObject.GetComponentInParent<IInteractable>();

            if (interactable != null)
            {
                Debug.Log("Interact Cast");
                if (_interactable == interactable) return;

                _interactable = interactable;
                interactable.StartedLooking(this);
            }
        }
        else
        {
            if (_interactable == null) return;
            _interactable.StoppedLooking(this);
            _interactable = null;
        }
    }

    void OnDrawGizmos()
    {
        if (!showDebug) return;

        if (!Player) return;
        Gizmos.DrawLine(Player.orientation.position, Player.orientation.position + Player.orientation.forward * range);
    }

}
