using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillCollider : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        PlayerCharacter player = GetComponentInParent<PlayerCharacter>();

        if (player)
        {
            player.Death();
        }
    }
}
