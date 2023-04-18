using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    public PlayerController controller { get; private set; }
    public Controls controls { get; protected set; }

    public System.Action<PlayerController> OnPossessed;
    public System.Action OnUnpossessed;

    public virtual void Possess(PlayerController controller)
    {
        this.controller = controller;
        controls = controller.controls;
        OnPossessed?.Invoke(controller);
    }

    public virtual void UnPossess()
    {
        this.controller = null;
        controls = null;
        OnUnpossessed?.Invoke();
    }

}
