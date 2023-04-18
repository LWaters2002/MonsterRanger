using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Hurtbox : MonoBehaviour
{
    public float damage;
    public float knockbackStrength;

    private Collider[] _colliders;

    private void Start()
    {
        CacheColliders();
    }

    private void CacheColliders()
    {
        _colliders = GetComponentsInChildren<Collider>();
        DeactivateColliders();
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerCharacter player = other.GetComponentInParent<PlayerCharacter>();
        if (!player) return;

        player.playerStats.AlterHealth(-damage);

        //Knockback
        Vector3 knockbackDirection = (-other.transform.position + transform.position).normalized;
        player.rb.AddForce(knockbackDirection * knockbackStrength, ForceMode.Impulse);
    }

    void ActivateForLength(float length)
    {
        Invoke("Deactivate", length);

        foreach (Collider collider in _colliders)
        {
            collider.enabled = true;
        }
    }

    public void ActivateColliders()
    {
        foreach (Collider collider in _colliders)
        {
            collider.enabled = true;
        }
    }

    public void DeactivateColliders()
    {
        foreach (Collider collider in _colliders)
        {
            collider.enabled = false;
        }
    }
}
