using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillCollider : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {

        PlayerCharacter pc = other.GetComponentInParent<PlayerCharacter>();

        if (pc)
        {
            pc.Death();
        }
    }
}
