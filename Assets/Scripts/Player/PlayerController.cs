using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Controls controls { get; private set; }

    // Start is called before the first frame update
    public void Init()
    {
        controls = new Controls();
        controls.Enable();
        Debug.Log("Controls Created : " + controls);

        Invoke("PossessDefaultPawn",.2f);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void PossessDefaultPawn()
    {
        Pawn pawn = FindObjectOfType<Pawn>();
        if (pawn) pawn.Possess(this);
    }

}
